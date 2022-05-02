using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Fantasy.Frame.Tests
{
    public class TestPluginManager
    {
        private class TestPlugin:APlugin
        {

            public readonly List<string> UseRecord = new();
            public TestPlugin(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
            {
                
            }

            public override string GetPluginName()
            {
                return nameof(TestPlugin);
            }

            public override void Install()
            {
                AddModule<TestModule>(new TestModule(PluginManager,true));
                UseRecord.Add(nameof(Install));
            }

            public override void Uninstall()
            {
                RemoveModule<TestModule>();
                UseRecord.Add(nameof(Uninstall));
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
        public void RegisteredAndUnRegistered()
        {
            var pluginManager=new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager,true);
            pluginManager.Registered(testPlugin);
            Assert.AreEqual(testPlugin.UseRecord ,new List<string> {nameof(TestPlugin.Install)});

            pluginManager.UnRegistered(testPlugin);
            Assert.AreEqual(testPlugin.UseRecord,new List<string>{nameof(TestPlugin.Install),nameof(TestPlugin.Uninstall)});
        }
        [UnityTest]
        public IEnumerator GetInitTimeAndGetInitTime()
        {
            var pluginManager=new PluginManager(null, false);
            var initTime = DateTime.Now.Ticks / 10000;
            pluginManager.Init();
            var type = typeof(PluginManager);
            var pluginManagerField = type.GetField("_initTime", BindingFlags.NonPublic | BindingFlags.Instance);
            var curInitTime =  (long) pluginManagerField.GetValue(pluginManager);
            Assert.IsTrue(curInitTime==initTime);
            yield return null;
            
            var nowTicks = DateTime.Now.Ticks / 10000;
            pluginManager.Execute();
            pluginManagerField = type.GetField("_nowTicks", BindingFlags.NonPublic | BindingFlags.Instance);
            var curNowTicks =  (long) pluginManagerField.GetValue(pluginManager);
            Assert.IsTrue(curNowTicks==nowTicks);
           
        }

        [Test]
        public void AddModule()
        {
            var pluginManager=new PluginManager(null, false);
            var test = new TestModule(pluginManager, true);
            
            var type = typeof(PluginManager);
            var testPluginsField = type.GetField("_modules", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = testPluginsField.GetValue(pluginManager) as Dictionary<string, AModule>;
            
            Assert.IsTrue(modules.Count==0);
            pluginManager.AddModule(typeof(TestModule).ToString(),test);
            Assert.IsTrue(modules.Count==1);

        }

        [Test]
        public void RemoveModule()
        {
            
            var pluginManager=new PluginManager(null, false);
            var test = new TestModule(pluginManager, true);
            
            var type = typeof(PluginManager);
            var testPluginsField = type.GetField("_modules", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = testPluginsField.GetValue(pluginManager) as Dictionary<string, AModule>;
            
            Assert.IsTrue(modules.Count==0);
            pluginManager.AddModule(typeof(TestModule).ToString(),test);
            Assert.IsTrue(modules.Count==1);
            pluginManager.RemoveModule(typeof(TestModule).ToString());
            Assert.IsTrue(modules.Count==0);
        }
        [Test]
        public void FindModule()
        {
            var pluginManager=new PluginManager(null, false);
            var test = new TestModule(pluginManager, true);
            pluginManager.AddModule(typeof(TestModule).ToString(),test);
            var findModule=pluginManager.FindModule<TestModule>();
            Assert.AreEqual(findModule, test);
        }
       

        [Test]
        public void BasicLifeCycle()
        {
            var pluginManager=new PluginManager(null, false);
            var testPlugin = new TestPlugin(pluginManager,true);
            pluginManager.Registered(testPlugin);
            
            var type = typeof(PluginManager);
            var testPluginsField = type.GetField("_updates", BindingFlags.NonPublic | BindingFlags.Instance);
            var modules = testPluginsField.GetValue(pluginManager) as AModule[];
            Assert.IsTrue(modules.Length==0);
            pluginManager.Awake();
            modules = testPluginsField.GetValue(pluginManager) as AModule[];
            Assert.IsTrue(modules.Length==1);
            pluginManager.Init();
            pluginManager.AfterInit();
            pluginManager.Execute();
            pluginManager.BeforeShut();
            pluginManager.Shut();
            
            var findModule=pluginManager.FindModule<TestModule>();
            Assert.IsTrue(findModule.Index==6);
        }
        
        
    }
}