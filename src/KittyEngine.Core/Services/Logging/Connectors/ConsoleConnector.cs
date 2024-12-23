
namespace KittyEngine.Core.Services.Logging.Conenctors
{
    public class ConsoleConnector : ILogConnector
    {
        public void Log(LogLevel level, string message)
        {
            var fullMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")} [{level}] {message}";
            Console.WriteLine(fullMessage);
        }
    }
}
