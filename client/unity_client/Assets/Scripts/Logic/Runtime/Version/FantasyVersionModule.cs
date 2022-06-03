using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Frame;
using Fantasy.Logic.Achieve;
using Fantasy.VersionInfo;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;

namespace Fantasy.Logic.Interface
{
    public class FantasyVersionModule: AModule, IFantasyVersionModule
    {
      
        
        private IFantasyLogModule _fantasyLogModule;
        private ILogger<FantasyVersionModule> _logger;
    
        
        private VersionInfoT _oldVersionInfoT;
        private VersionInfoT _newVersionInfoT;
    
        
#if FANTASY_USE_BIN_FILE
        public const string VersionInfoName = "version_info.bin";
#else
        public const string VersionInfoName = "version_info.json";
#endif

        private bool _init;
        private bool _initUpdate;
        
        private IFantasyConfigModule _fantasyConfigModule;
        private IFantasyAssetModule _fantasyAssetModule;

        
        public FantasyVersionModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            FantasyAssetPath.Init();
            ZString.Format("{0} :{1}", nameof(FantasyAssetPath.LocalResourceDirectory), FantasyAssetPath.LocalResourceDirectory).ZLogDebug();
            ZString.Format("{0} :{1}", nameof(FantasyAssetPath.CacheResourceDirectory), FantasyAssetPath.CacheResourceDirectory).ZLogDebug();
        }
        
        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            _fantasyConfigModule = PluginManager.FindModule<IFantasyConfigModule>() as IFantasyConfigModule;
            _fantasyAssetModule= PluginManager.FindModule<IFantasyAssetModule>() as IFantasyAssetModule;
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyVersionModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyVersionModule));
            LoadData().Forget();
        }
        private async UniTaskVoid LoadData()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(5));
            await UniTask.SwitchToThreadPool();
            var path = ZString.Concat(FantasyAssetPath.CacheResourceDirectory, VersionInfoName);
            if (!File.Exists(path))
            {
                path = ZString.Concat(FantasyAssetPath.LocalResourceDirectory, VersionInfoName);
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
                    var response = await client.GetAsync(url, cts.Token);
                    response.EnsureSuccessStatusCode();
                    _newVersionInfoT = await ReadNetReadVersionInfoT(response);
                    _logger.ZLogDebug("_newVersionInfoT : \n{0}", _newVersionInfoT.SerializeToJson());
                }
                catch (TaskCanceledException)
                {
                    var url = ZString.Concat(versionInfoT.Url, VersionInfoName);
                    _logger.ZLogError("request timed out {0}", url);
                }
            }
            await UniTask.SwitchToMainThread();
            _initUpdate = true;
        }
        
        public bool GetInitSuccessful()
        {
            return _init;
        }

        public bool GetUpdateSuccessful()
        {
            return _initUpdate;
        }

        public void StartUpdate()
        {
            _logger.ZLogDebug("{0}   StartUpdate", nameof(FantasyVersionModule));
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

        

        private async UniTask UpdateDataAsync()
        {
            if (_oldVersionInfoT.DataVersion != _newVersionInfoT.DataVersion)
            {
                await _fantasyConfigModule.UpdateData(_oldVersionInfoT.Url);
            }
        }

        private async UniTask UpdateAssetBundleAsync()
        {
            if (_oldVersionInfoT.AssetVersion != _newVersionInfoT.AssetVersion)
            {
                await _fantasyAssetModule.UpdateData(_oldVersionInfoT.Url);
            }
        }
        public VersionInfoT GetOldVersionInfoT()
        {
            return _oldVersionInfoT;
        }

        public VersionInfoT GetNewVersionInfoT()
        {
            return _newVersionInfoT;
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
    }
}