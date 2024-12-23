
namespace KittyEngine.Core.Services.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
