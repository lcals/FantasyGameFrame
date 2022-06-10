using Cysharp.Threading.Tasks;

namespace Fantasy.Logic.Interface
{
    public interface IFantasyConfigModule
    {
        UniTask UpdateData(string url);
    }
}