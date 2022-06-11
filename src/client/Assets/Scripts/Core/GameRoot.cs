using Fantasy.Frame;
using Fantasy.Logic.Achieve;
using UnityEngine;

namespace Fantasy.Logic
{
    public class GameRoot : MonoBehaviour
    {
        private static PluginManager _pluginManager;

        public static bool Successful;

        public static PluginManager GetPluginManager()
        {
            return _pluginManager;
        }

        private void Awake()
        {
            Debug.Log("game awake");
            _pluginManager = new PluginManager(null, false);
            Debug.Log("_pluginManager Registered");
            _pluginManager.Registered();
            "pluginManager Awake".ZLogDebug();
            _pluginManager.Awake();
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
            Debug.Log("_pluginManager UnRegistered");
            _pluginManager.UnRegistered();
        }
    }
}