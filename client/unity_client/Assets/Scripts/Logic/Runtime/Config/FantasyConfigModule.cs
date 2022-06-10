using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Config;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.Networking;
using ZLogger;

namespace Fantasy.Logic.Achieve
{
    public class FantasyConfigModule : AModule, IFantasyConfigModule
    {
        private IFantasyLogModule _fantasyLogModule;
        private ILogger<FantasyConfigModule> _logger;

        private const string ConfigPath = "config/config.bytes";

        public FantasyConfigModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            
        }

        public async UniTask UpdateData(string url)
        {
            var fullUrl = ZString.Concat(url, ConfigPath);
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
            Config.ConfigRootT = ConfigRootT.DeserializeFromBinary(bytes);
            _logger.ZLogDebug("ConfigRootT.Version {0}", Config.ConfigRootT.Version.ToString());
        }

        #endregion

        #region

        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyConfigModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyConfigModule));
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