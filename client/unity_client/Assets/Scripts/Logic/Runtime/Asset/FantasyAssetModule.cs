using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Frame;
using Fantasy.HashCacheInfo;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.Networking;
using ZLogger;

namespace Fantasy.Logic.Achieve
{
    public class FantasyAssetModule : AModule, IFantasyAssetModule
    {
#if UNITY_EDITOR
#if FANTASY_USE_ASSET_BUNDLE
        public const bool IsUseAssetBundle = true;
#else
        public const bool IsUseAssetBundle = false;
#endif
#else
        public const bool IsUseAssetBundle = true;
#endif

        private IFantasyLogModule _fantasyLogModule;
        private ILogger<FantasyAssetModule> _logger;
        private AssetInfoT _assetInfoT;

        private const string AssetInfoPath = "asset/asset_info.bytes";

        public FantasyAssetModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
        }

        public async UniTask UpdateData(string url)
        {
            var fullUrl = ZString.Concat(url, AssetInfoPath);
            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(5));
            try
            {
                var progress = Progress.Create<float>(x => { _logger.ZLogDebug("request progress :{0}", x); });
                _logger.ZLogDebug("update url : \n{0}", fullUrl);
                var unityWebRequest = await UnityWebRequest.Get(fullUrl).SendWebRequest()
                    .ToUniTask(progress, PlayerLoopTiming.Update, cts.Token);
                LoadData(unityWebRequest.downloadHandler.data);
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token) _logger.ZLogError("request timed out {0}", fullUrl);
            }
        }

        #region Help

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadData(byte[] bytes)
        {
            _assetInfoT = AssetInfoT.DeserializeFromBinary(bytes);
            _logger.ZLogDebug("ConfigRootT.Version {0}", _assetInfoT.Version.ToString());
        }

        #endregion


        #region

        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyAssetModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyAssetModule));
        }

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