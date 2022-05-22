using Fantasy.Frame;
using Fantasy.Logic.Achieve;
using Fantasy.Logic.Interface;

namespace Fantasy.Logic
{
    public class GameBaseModulePlugin:APlugin
    {
        public GameBaseModulePlugin(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            
        }

        public override string GetPluginName()
        {
            return nameof(GameBaseModulePlugin);
        }
        
        public override void Install()
        {
            AddModule<IFantasyLogModule>(new FantasyLogModule(PluginManager,false));
            AddModule<IFantasyConfigModule>(new FantasyConfigModule(PluginManager,true));
            AddModule<IFantasyAssetModule>(new FantasyAssetModule(PluginManager,true));
        }

        public override void Uninstall()
        {
            RemoveModule<IFantasyAssetModule>();
            RemoveModule<IFantasyConfigModule>();
            RemoveModule<IFantasyLogModule>();
           
        }
    }
}