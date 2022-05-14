using Fantasy.Frame;
using Fantasy.Logic.Achieve;
using Fantasy.Logic.Interface;

namespace Fantasy.Logic
{
    public static class PluginAndModuleInfo
    {
        public static void Install(this PluginManager pluginManager)
        {
            pluginManager.Registered(new TestPlugin(pluginManager,false));
            pluginManager.Registered(new GameBaseModulePlugin(pluginManager,false));
            
        }
        
    }
}