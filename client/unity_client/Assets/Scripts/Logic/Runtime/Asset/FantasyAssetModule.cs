using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Fantasy.VersionInfo;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;

namespace Fantasy.Logic.Achieve
{
    public class FantasyAssetModule : AModule, IFantasyAssetModule
    {
#if FANTASY_USE_BIN_FILE
        public const string VersionInfoName = "version_info.bin";
#else
        public const string VersionInfoName = "version_info.json";
#endif

#if UNITY_EDITOR
#if FANTASY_USE_ASSET_BUNDLE
        public const bool IsUseAssetBundle = true;
#else
        public const bool IsUseAssetBundle = false;
#endif
#else
        public const bool IsUseAssetBundle = true;
#endif

        public const string ConfigPath = "Config/config.bytes";

        private readonly string _localResourceDirectory;
        private readonly string _cacheResourceDirectory;
        private readonly string _rootPath;

        

        private IFantasyLogModule _fantasyLogModule;
        private ILogger<FantasyAssetModule> _logger;

        private bool _init;
        private bool _initUpdate;
        private VersionInfoT _oldVersionInfoT;
        private VersionInfoT _newVersionInfoT;
        private CancellationTokenSource _cts;
        private IFantasyConfigModule _fantasyConfigModule;


        public FantasyAssetModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    _rootPath = $"{Application.streamingAssetsPath}/../AssetDirectory~/";
                    _localResourceDirectory = $"{Application.streamingAssetsPath}/../AssetDirectory~/LocalResource/";
                    _cacheResourceDirectory = $"{Application.streamingAssetsPath}/../AssetDirectory~/CacheResource/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                    _rootPath = $"{Application.streamingAssetsPath}/";
                    _localResourceDirectory = $"{Application.streamingAssetsPath}/";
                    _cacheResourceDirectory = $"{Application.persistentDataPath}/";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ZString.Format("{0} :{1}", nameof(_rootPath), _rootPath).ZLogDebug();
            ZString.Format("{0} :{1}", nameof(_localResourceDirectory), _localResourceDirectory).ZLogDebug();
            ZString.Format("{0} :{1}", nameof(_cacheResourceDirectory), _cacheResourceDirectory).ZLogDebug();
        }

        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyAssetModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyConfigModule));
            LoadData().Forget();
        }

        private async UniTaskVoid LoadData()
        {
            _cts = new CancellationTokenSource();
            _cts.CancelAfterSlim(TimeSpan.FromSeconds(5));
            await UniTask.SwitchToThreadPool();
            var path = ZString.Concat(_rootPath, VersionInfoName);
            if (!File.Exists(path))
            {
                path = ZString.Concat(_rootPath, VersionInfoName);
            }
            var versionInfoT = ReadVersionInfoT(path);
            _oldVersionInfoT = versionInfoT;
            _init = true;
            _logger.ZLogDebug("VersionInfoT : \n{0}", versionInfoT.SerializeToJson());
            if (_oldVersionInfoT.Update)
            {
                try
                {
                    using var client = new HttpClient();
                    var url = ZString.Concat(versionInfoT.Url, VersionInfoName);
                    _logger.ZLogDebug("update url : \n{0}", url);
                    var response = await client.GetAsync(url, _cts.Token);
                    response.EnsureSuccessStatusCode();
                    _newVersionInfoT = await ReadNetReadVersionInfoT(response);
                    _logger.ZLogDebug("_newVersionInfoT : \n{0}", _newVersionInfoT.SerializeToJson());
                }
                catch (TaskCanceledException)
                {
                    var url = ZString.Concat(versionInfoT.Url, VersionInfoName);
                    _logger.ZLogError("Request timed out {0}",url);
                }   
            }
            await UniTask.SwitchToMainThread();
            _initUpdate = true;
        }

        #region Help

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static VersionInfoT ReadVersionInfoT(string path)
        {
#if FANTASY_USE_BIN_FILE
            return VersionInfoT.DeserializeFromBinary(File.ReadAllBytes(path));
#else
            return VersionInfoT.DeserializeFromJson(File.ReadAllText(path));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<VersionInfoT> ReadNetReadVersionInfoT(HttpResponseMessage response)
        {
#if FANTASY_USE_BIN_FILE
            return VersionInfoT.DeserializeFromBinary(await response.Content.ReadAsByteArrayAsync());
#else
            return VersionInfoT.DeserializeFromJson(await response.Content.ReadAsStringAsync());
#endif
        }

        #endregion

        #region

        public override void Init()
        {
        }

        public override void AfterInit()
        {
        }

        public override void Execute()
        {
        }

        public override void BeforeShut()
        {
        }

        public override void Shut()
        {
        }

        #endregion
        
        public string GetLocalResourceDirectory()
        {
            return _localResourceDirectory;
        }
        public string GetCacheResourceDirectory()
        {
            return _cacheResourceDirectory;
        }
        public bool GetInitSuccessful()
        {
            return _init;
        }

        public bool GetUpdateSuccessful()
        {
            return _initUpdate;
        }

        public VersionInfoT GetOLdVersionInfoT()
        {
            return _oldVersionInfoT;
        }

        public VersionInfoT GetNewVersionInfoT()
        {
            return _newVersionInfoT;
        }

        public void StartUpdate()
        {
            _logger.ZLogDebug("{0}   StartUpdate", nameof(FantasyConfigModule));
            _fantasyConfigModule= PluginManager.FindModule<IFantasyConfigModule>() as  IFantasyConfigModule;
            UpdateGameAsync().Forget();
        }
        private async UniTaskVoid UpdateGameAsync()
        {
            await UniTask.SwitchToThreadPool();
            var updateData = UpdateDataAsync();
            var updateAssetBundle = UpdateAssetBundleAsync();
            await (updateData, updateAssetBundle);
            await UniTask.SwitchToMainThread();
        }
        private  async UniTask UpdateDataAsync( )
        {
            if (_oldVersionInfoT.DataVersion!=_newVersionInfoT.DataVersion)
            {
                try
                {
                    using var client = new HttpClient();
                    var url = ZString.Concat(_newVersionInfoT.Url, ConfigPath);
                    _logger.ZLogDebug("ConfigPath url : \n{0}", url);
                    // var response = await client.GetAsync(url, _cts.Token);
                    // response.EnsureSuccessStatusCode();
                    // var bytes=   await response.Content.ReadAsByteArrayAsync();
                     var bytes = await client.GetByteArrayAsync(url);
                    _fantasyConfigModule.LoadData(bytes);
                }
                catch (TaskCanceledException)
                {
                    var url = ZString.Concat(_newVersionInfoT.Url, ConfigPath);
                    _logger.ZLogError("Request timed out {0}",url);
                }   
            }
        }
        private  async UniTask UpdateAssetBundleAsync( )
        {
            await UniTask.Delay(0);
        }
    }
}