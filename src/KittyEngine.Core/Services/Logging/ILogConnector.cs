
namespace KittyEngine.Core.Services.Logging
{
    public interface ILogConnector
    {
        void Log(LogLevel level, string message);
    }
}
