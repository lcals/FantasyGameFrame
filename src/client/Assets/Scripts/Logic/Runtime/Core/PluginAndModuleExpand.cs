using Fantasy.Frame;

namespace Fantasy.Logic
{
    public static class PluginAndModuleExpand
    {
        private static GameBaseModulePlugin _gameBaseModulePlugin;
        public static void Registered(this PluginManager pluginManager)
        {
            _gameBaseModulePlugin = new GameBaseModulePlugin(pluginManager, false);
            pluginManager.Registered(_gameBaseModulePlugin);
        }
        public static void UnRegistered(this PluginManager pluginManager)
        {
            pluginManager.UnRegistered(_gameBaseModulePlugin);
            _gameBaseModulePlugin = null;
        }
    }
}