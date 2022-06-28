using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Fantasy.Frame
{
    public sealed class PluginManager : AModule
    {
        private long _initTime;
        private long _nowTicks;
        private readonly Dictionary<string, APlugin> _plugins = new();
        private readonly Dictionary<string, AModule> _modules = new();
        private AModule[] _updates = Array.Empty<AModule>();
        private bool _isAwakeEnd;

        public PluginManager(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
        }

        public override void Awake()
        {
            _updates = _modules.Values.Where(x => x.IsUpdate).ToArray();
            _isAwakeEnd = true;
            foreach (var plugin in _plugins.Values) plugin.Awake();
        }

        public override void Init()
        {
            _initTime = DateTime.Now.Ticks / 10000;
            foreach (var plugin in _plugins.Values) plugin?.Init();
        }

        public override void AfterInit()
        {
            foreach (var plugin in _plugins.Values) plugin?.AfterInit();
        }

        public override void Execute()
        {
            _nowTicks = DateTime.Now.Ticks / 10000;
            foreach (var update in _updates) update.Execute();
        }

        public override void BeforeShut()
        {
            foreach (var plugin in _plugins.Values) plugin?.BeforeShut();
        }

        public override void Shut()
        {
            foreach (var plugin in _plugins.Values) plugin?.Shut();
        }
        
        public void Registered(APlugin plugin)
        {
            _plugins.Add(plugin.GetPluginName(), plugin);
            plugin.Install();
        }

        public void UnRegistered(APlugin plugin)
        {
            _plugins.Remove(plugin.GetPluginName());
            plugin.Uninstall();
        }

        public long GetInitTime()
        {
            return _initTime;
        }

        public long GetNowTime()
        {
            return _nowTicks;
        }

        public void AddModule(string moduleName, AModule module)
        {
            _modules.Add(moduleName, module);
            if (_isAwakeEnd&&module.IsUpdate) _updates = _modules.Values.ToArray();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveModule(string moduleName)
        {
            var module = _modules[moduleName];
            _modules.Remove(moduleName);
            if (module.IsUpdate) _updates = _modules.Values.ToArray();
        }
        public T FindModule<T>()
        {
            var moduleName = typeof(T).ToString();
            return _modules.TryGetValue(moduleName, out var module) ? module is T value ? value : default: default ;
        }

       
    }
}