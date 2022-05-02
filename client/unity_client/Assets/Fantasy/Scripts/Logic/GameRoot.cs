using Fantasy.Frame;
using UnityEngine;
namespace Fantasy.Logic
{
    
    public class GameRoot:  MonoBehaviour
    {
        private static PluginManager _pluginManager;
        
        public static PluginManager GetPluginManager()
        {
            return _pluginManager;
        }

        private void Awake()
        {
            _pluginManager = new PluginManager(null,false);
            _pluginManager.Install();
            _pluginManager.Awake();
            var test = _pluginManager.FindModule<ITestModule>() as ITestModule; 
            test.Test();;
        }

        private void Start()
        {
            _pluginManager.Init();
            _pluginManager.AfterInit();
            
        }

        private void Update()
        {
            _pluginManager.Execute();
        }
        
        private void OnDisable()
        {
            _pluginManager.BeforeShut();
            _pluginManager.Shut();
        }
        
    }
}