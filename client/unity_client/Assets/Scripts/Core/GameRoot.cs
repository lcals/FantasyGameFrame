using Fantasy.Frame;
using Fantasy.Logic.Achieve;
using UnityEngine;

namespace Fantasy.Logic
{
    public class GameRoot : MonoBehaviour
    {
        private static PluginManager _pluginManager;

        public static bool Successful = false;
        public static PluginManager GetPluginManager()
        {
            return _pluginManager;
        }

        private void Awake()
        {
            Debug.Log("game awake");
            _pluginManager = new PluginManager(null, false);
            Debug.Log("_pluginManager Install");
            _pluginManager.Install();
            "pluginManager Awake".ZLogDebug();
            _pluginManager.Awake();
            var test = _pluginManager.FindModule<ITestModule>() as ITestModule;
            test.Test();
            
        }

        private void Start()
        {
            "pluginManager Init".ZLogDebug();
            _pluginManager.Init();
            "pluginManager AfterInit".ZLogDebug();
            _pluginManager.AfterInit();
            Successful = true;

        }

        private void Update()
        {
            _pluginManager.Execute();
        }

        private void OnDisable()
        {
            "pluginManager BeforeShut".ZLogDebug();
            _pluginManager.BeforeShut();
            "pluginManager Shut".ZLogDebug();
            _pluginManager.Shut();
        }
    }
}