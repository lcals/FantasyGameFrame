using Microsoft.Extensions.Logging;

namespace Fantasy.Logic.Interface
{
    public interface IFantasyLogModule
    {
        public ILogger<T> GetLogger<T>() where T : class;
        public ILogger GetLogger(string categoryName);
    }
}