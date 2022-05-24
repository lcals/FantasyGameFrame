namespace Fantasy.Logic.Interface
{
    public interface IFantasyConfigModule
    {
        void LoadData(byte[] bytes);
        void UpdateData();
    }
}