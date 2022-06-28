namespace Fantasy.Logic.Achieve
{
    public interface IFantasyUIModule
    {
        void OpenUI<T>() where T:AFantasyBaseUI;
        void CloseUI<T>() where T:AFantasyBaseUI;
        T GetUI<T>() where T:AFantasyBaseUI;
    }
}