using Fantasy.Frame;
using UnityEngine;

namespace Fantasy.Logic
{
    public class TestModule:AModule
    {
        public TestModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
        }

        public override void Awake()
        {
            Debug.Log(nameof(Awake));
        }

        public override void Init()
        {
            Debug.Log(nameof(Init));

        }

        public override void AfterInit()
        {
            Debug.Log(nameof(AfterInit));

        }

        public override void Execute()
        {
            Debug.Log(nameof(Execute));

        }

        public override void BeforeShut()
        {
            Debug.Log(nameof(BeforeShut));

        }

        public override void Shut()
        {
            Debug.Log(nameof(Shut));

        }
    }
}