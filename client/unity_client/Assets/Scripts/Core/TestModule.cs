using Fantasy.Frame;
using UnityEngine;

namespace Fantasy.Logic
{
    public interface ITestModule
    {
        void Test();
    }

    public class TestModule : AModule, ITestModule
    {
        public TestModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
        }

        public override void Awake()
        {
            Debug.Log($"TestModule {nameof(Awake)}");
        }

        public override void Init()
        {
            Debug.Log($"TestModule {nameof(Init)}");
        }

        public override void AfterInit()
        {
            Debug.Log($"TestModule {nameof(AfterInit)}");
        }

        public override void Execute()
        {
        }

        public override void BeforeShut()
        {
            Debug.Log($"TestModule {nameof(BeforeShut)}");
        }

        public override void Shut()
        {
            Debug.Log($"TestModule {nameof(Shut)}");
        }

        public void Test()
        {
            Debug.Log($"TestModule {nameof(Test)}");
        }
    }
}