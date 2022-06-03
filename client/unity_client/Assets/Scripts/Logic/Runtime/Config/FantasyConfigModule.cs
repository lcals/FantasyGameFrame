using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Config;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using ZLogger;
using Debug = System.Diagnostics.Debug;

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
            var  fullUrl = ZString.Concat(url, ConfigPath);
            try
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfterSlim(TimeSpan.FromSeconds(5));
                using var client = new HttpClient();
                _logger.ZLogDebug("ConfigPath url : \n{0}", fullUrl);
                var response = await client.GetAsync(fullUrl, cts.Token);
                response.EnsureSuccessStatusCode();
                var bytes = await response.Content.ReadAsByteArrayAsync();
                LoadData(bytes);

            }
            catch (TaskCanceledException)
            {
                _logger.ZLogError("Request timed out {0}", fullUrl);
            }
        }

        #region Help

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadData(byte[] bytes)
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
            _logger.ZLogDebug("{0}   Awake",nameof(FantasyConfigModule));
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