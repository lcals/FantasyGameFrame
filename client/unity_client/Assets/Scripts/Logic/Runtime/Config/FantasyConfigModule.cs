using System.Runtime.CompilerServices;
using Fantasy.Config;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;
using Debug = System.Diagnostics.Debug;

namespace Fantasy.Logic.Achieve
{
    public class FantasyConfigModule : AModule, IFantasyConfigModule
    {
        private IFantasyLogModule _fantasyLogModule;
        private ILogger<FantasyConfigModule> _logger;
        public FantasyConfigModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadData()
        {
            var  textAsset= Resources.Load<TextAsset>("Config/config");
            Config.ConfigRootT = ConfigRootT.DeserializeFromBinary(textAsset.bytes);
        }

        public void UpdateData()
        {
            LoadData();
        }
        
        #region 

        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyConfigModule>();
            _logger.ZLogDebug("{0}   Awake",nameof(FantasyConfigModule));
             LoadData();
            _logger.ZLogDebug("ConfigRootT.Version {0}", Config.ConfigRootT.Version.ToString());
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