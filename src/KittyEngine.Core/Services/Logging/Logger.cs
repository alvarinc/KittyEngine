
namespace KittyEngine.Core.Services.Logging
{
    public class Logger : ILogger
    {
        private ILoggerManager _loggerManager;

        public Logger(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }

        public void Log(LogLevel level, string message)
        {
            if (_loggerManager.IsEnabled)
            {
                foreach(var connector in _loggerManager.Connectors)
                {
                    connector.Log(level, message);
                }
            }
        }
    }
}
