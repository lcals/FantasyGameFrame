namespace Fantasy.Frame
{
    public abstract class AModule
    {
        protected readonly PluginManager PluginManager;
        protected internal readonly bool IsUpdate;

        protected AModule(PluginManager pluginManager, bool isUpdate)
        {
            PluginManager = pluginManager;
            IsUpdate = isUpdate;
        }

        public abstract void Awake();
        public abstract void Init();
        public abstract void AfterInit();
        public abstract void Execute();
        public abstract void BeforeShut();
        public abstract void Shut();
    }
}