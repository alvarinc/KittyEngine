
using Serilog;

namespace KittyEngine.Core.Services.Logging.Conenctors
{
    public class SerilogConnector : ILogConnector
    {
        private readonly Serilog.Core.Logger _logger;

        public SerilogConnector() 
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File("log-kitty.txt", rollingInterval: RollingInterval.Day, shared:true)
                .CreateLogger();
        }

        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
                case LogLevel.Warn:
                    _logger.Warning(message);
                    break;
                case LogLevel.Info:
                    _logger.Information(message);
                    break;
            }
        }
    }
}
