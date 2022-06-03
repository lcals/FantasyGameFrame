using Cysharp.Threading.Tasks;

namespace Fantasy.Logic.Interface
{
    public interface IFantasyConfigModule
    {
        void LoadData(byte[] bytes);
        UniTask UpdateData(string url);
    }
}