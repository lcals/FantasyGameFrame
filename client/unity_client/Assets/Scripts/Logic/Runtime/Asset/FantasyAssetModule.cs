using Cysharp.Threading.Tasks;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;

namespace Fantasy.Logic.Achieve
{
    public partial class FantasyAssetModule : AModule, IFantasyAssetModule
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

        public async UniTask UpdateData(string url)
        {
            await   UniTask.Delay(0);
        }
        


        public FantasyAssetModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
          
        }

        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyAssetModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyAssetModule));
          
        }
        

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