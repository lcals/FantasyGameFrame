using Fantasy.Frame;

namespace Fantasy.Logic
{
    public class TestPlugin:APlugin
    {
        public TestPlugin(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
        }

        public override string GetPluginName()
        {
            return nameof(TestPlugin);
        }

        public override void Install()
        {
            AddModule<ITestModule>(new TestModule(PluginManager,true));
        }

        public override void Uninstall()
        {
           RemoveModule<ITestModule>();
        }
    }
}