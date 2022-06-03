using Cysharp.Threading.Tasks;

namespace Fantasy.Logic.Achieve
{
    public interface IFantasyAssetModule
    {
        UniTask UpdateData(string url);
    }
}