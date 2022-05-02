using Fantasy.Frame;

namespace Fantasy.Logic
{
    public static class PluginAndModuleInfo
    {
        public static void Install(this PluginManager pluginManager)
        {
            pluginManager.Registered(new TestPlugin(pluginManager,false));
        }
        
    }
}