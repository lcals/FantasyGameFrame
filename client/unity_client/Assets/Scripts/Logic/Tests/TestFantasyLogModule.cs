
    using Fantasy.Frame;
    using Fantasy.Logic.Achieve;
    using Fantasy.Logic.Interface;
    using NUnit.Framework;
    using ZLogger;

    public class TestFantasyLogModule
    {
        private class TestPlugin:APlugin
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
                AddModule<IFantasyLogModule>(new FantasyLogModule(PluginManager,false));
            }

            public override void Uninstall()
            {
                RemoveModule<IFantasyLogModule>();
            }
        }
        [Test]
        public void Logger()
        {   
            var pluginManager=new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager,true);
            pluginManager.Registered(testPlugin);
            FantasyLogModule.Logger.ZLogDebug("Test Logger");

        }
        [Test]
        public void GetLogger()
        {
            var pluginManager=new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager,true);
            pluginManager.Registered(testPlugin);
            IFantasyLogModule  findModule=pluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            findModule.GetLogger<TestPlugin>().ZLogDebug("Test GetLogger");
        }
        [Test]
        public void GetLoggerCategoryName()
        {
            var pluginManager=new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager,true);
            pluginManager.Registered(testPlugin);
            IFantasyLogModule  findModule=pluginManager.FindModule<IFantasyLogModule>() as FantasyLogModule;
            findModule.GetLogger("CategoryName").ZLogDebug("Test GetLoggerCategoryName");
        }
    }

    
