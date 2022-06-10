using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace Fantasy.Frame.Tests
{
    public class TestAPlugin
    {
        private class TestPlugin : APlugin
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
            }

            public override void Uninstall()
            {
            }
        }

        private class TestModule : AModule
        {
            public int Index;

            public TestModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
            {
            }

            public override void Awake()
            {
                Index++;
            }

            public override void Init()
            {
                Index++;
            }

            public override void AfterInit()
            {
                Index++;
            }

            public override void Execute()
            {
                Index++;
            }

            public override void BeforeShut()
            {
                Index++;
            }

            public override void Shut()
            {
                Index++;
            }
        }

        [Test]
        public void AddModule()
        {
            var pluginManager = new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager, true);


            var type = typeof(TestPlugin).BaseType;
            var testPluginsField = type.GetField("_modules", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = testPluginsField.GetValue(testPlugin) as Dictionary<string, AModule>;

            Assert.IsTrue(modules.Count == 0);
            testPlugin.AddModule<TestModule>(new TestModule(pluginManager, true));
            Assert.IsTrue(modules.Count == 1);
        }

        [Test]
        public void RemoveModule()
        {
            var pluginManager = new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager, true);

            var type = typeof(TestPlugin).BaseType;
            var testPluginsField = type.GetField("_modules", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = testPluginsField.GetValue(testPlugin) as Dictionary<string, AModule>;

            Assert.IsTrue(modules.Count == 0);


            testPlugin.AddModule<TestModule>(new TestModule(pluginManager, true));
            Assert.IsTrue(modules.Count == 1);
            testPlugin.RemoveModule<TestModule>();
            Assert.IsTrue(modules.Count == 0);


            testPlugin.AddModule<TestModule>(new TestModule(pluginManager, true));
            Assert.IsTrue(modules.Count == 1);
            testPlugin.RemoveModule<TestModule>();
            Assert.IsTrue(modules.Count == 0);
        }

        [Test]
        public void BasicLifeCycle()
        {
            var pluginManager = new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager, true);
            var testModule = new TestModule(pluginManager, true);
            testPlugin.AddModule<TestModule>(testModule);

            testPlugin.Awake();
            testPlugin.Init();
            testPlugin.AfterInit();
            testPlugin.BeforeShut();
            testPlugin.Shut();
            Assert.IsTrue(testModule.Index == 5);
        }
    }
}