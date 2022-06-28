using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Frame;
using Fantasy.Logic.Achieve;
using Fantasy.VersionInfo;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.Networking;
using ZLogger;

namespace Fantasy.Logic.Interface
{
    public class FantasyVersionModule : AModule, IFantasyVersionModule
    {
        private IFantasyLogModule _fantasyLogModule;
        private IFantasyConfigModule _fantasyConfigModule;
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
        private bool _updateGame;

        public FantasyVersionModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            FantasyAssetPath.Init();
            ZString.Format("{0} :{1}", nameof(FantasyAssetPath.LocalResourceDirectory),
                FantasyAssetPath.LocalResourceDirectory).ZLogDebug();
            ZString.Format("{0} :{1}", nameof(FantasyAssetPath.CacheResourceDirectory),
                FantasyAssetPath.CacheResourceDirectory).ZLogDebug();
        }

        public override void Awake()
        {
            _fantasyConfigModule = PluginManager.FindModule<IFantasyConfigModule>();
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>();
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyVersionModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyVersionModule));
            LoadData().Forget();
        }


        private async UniTaskVoid LoadData()
        {
            var path = ZString.Concat(FantasyAssetPath.CacheResourceDirectory, VersionInfoName);
            if (!File.Exists(path)) path = ZString.Concat(FantasyAssetPath.LocalResourceDirectory, VersionInfoName);
            _oldVersionInfoT = ReadVersionInfoT(path);
            _init = true;
            _logger.ZLogDebug("VersionInfoT : \n{0}", _oldVersionInfoT.SerializeToJson());
            if (_oldVersionInfoT.Update)
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfterSlim(TimeSpan.FromSeconds(5));
                try
                {
                    var progress = Progress.Create<float>(x => { _logger.ZLogDebug("request progress :{0}", x); });
                    var url = ZString.Concat(_oldVersionInfoT.Url, VersionInfoName);
                    _logger.ZLogDebug("update url : \n{0}", url);
                    var unityWebRequest = await UnityWebRequest.Get(url).SendWebRequest()
                        .ToUniTask(progress, PlayerLoopTiming.Update, cts.Token);
                    _newVersionInfoT = ReadNetReadVersionInfoT(unityWebRequest.downloadHandler);
                }
                catch (OperationCanceledException ex)
                {
                    if (ex.CancellationToken == cts.Token)
                    {
                        var url = ZString.Concat(_oldVersionInfoT.Url, VersionInfoName);
                        _logger.ZLogError("request timed out {0}", url);
                    }
                }
                catch (Exception e)
                {
                    _logger.ZLogError("Exception :", e);
                }

               
            }

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
        public bool GetGameUpdateSuccessful()
        {
            return _updateGame;
        }
        
        public void StartUpdate()
        {
            _logger.ZLogDebug("{0}   StartUpdate", nameof(FantasyVersionModule));
            UpdateGameAsync().Forget();
        }


        private async UniTaskVoid UpdateGameAsync()
        {
            var updateData = UpdateDataAsync();
            var updateAssetBundle = UpdateAssetBundleAsync();
            await (updateData, updateAssetBundle);
            _updateGame = true;

        }


        private async UniTask UpdateDataAsync()
        {
            if (_oldVersionInfoT.DataVersion != _newVersionInfoT.DataVersion)
                await _fantasyConfigModule.UpdateData(_oldVersionInfoT.Url);
        }

        private async UniTask UpdateAssetBundleAsync()
        {
           
            if (_oldVersionInfoT.AssetVersion != _newVersionInfoT.AssetVersion)
                await UniTask.Delay(0);
            
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
        private static VersionInfoT ReadNetReadVersionInfoT(DownloadHandler downloadHandler)
        {
#if FANTASY_USE_BIN_FILE
            return VersionInfoT.DeserializeFromBinary(downloadHandler.data);
#else
            return VersionInfoT.DeserializeFromJson(downloadHandler.text);
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