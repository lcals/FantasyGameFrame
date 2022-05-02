using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fantasy.Frame
{
    public abstract class APlugin : AModule
    {
        
        private readonly Dictionary<string, AModule> _modules = new();
        public abstract string GetPluginName();
        public abstract void Install();
        public abstract void Uninstall();
        
        protected APlugin(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            
        }
        public override void Awake()
        {
            foreach (var module in _modules.Values) module?.Awake();
        }

        public override void Init()
        {
            foreach (var module in _modules.Values) module?.Init();
        }

        public override void AfterInit()
        {
            foreach (var module in _modules.Values) module?.AfterInit();
        }

        public override void Execute()
        {
            
        }

        public override void BeforeShut()
        {
            foreach (var module in _modules.Values) module?.BeforeShut();
        }

        public override void Shut()
        {
            foreach (var module in _modules.Values) module?.Shut();
        }

        public void AddModule<T>(AModule module)
        {
            var moduleName = typeof(T).ToString();
#if FANTASY_SECURITY_CHECK
            if (_modules.ContainsKey(moduleName)) return;
#endif
            _modules.Add(moduleName, module);
            PluginManager.AddModule(moduleName, module);
        }
        
        public void RemoveModule<T>()
        {
            var moduleName = typeof(T).ToString();
            RemoveModule(moduleName);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveModule(string moduleName)
        {
#if FANTASY_SECURITY_CHECK
            if (!_modules.ContainsKey(moduleName)) return;
#endif
            _modules.Remove(moduleName);
            PluginManager.RemoveModule(moduleName);
        }
        
    

     
    }
}