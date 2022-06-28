using MagicOnion;
using MessagePack;

namespace Fantasy.Logic.ServerShared
{
    /// <summary>
    /// Client -> Server API
    /// </summary>
    public interface IChatService : IService<IChatService>
    {
        UnaryResult<Nil> GenerateException(string message);
        UnaryResult<Nil> SendReportAsync(string message);
    }
}
