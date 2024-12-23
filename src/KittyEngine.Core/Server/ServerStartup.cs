using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.Services.Logging.Conenctors;

namespace KittyEngine.Core.Server
{
    public static class ServerStartup
    {
        public static IServiceContainer ConfigureGameServer(this IServiceContainer container)
        {
            // IoC
            container.Register<IServiceContainer>(ServiceBehavior.Scoped, () => container);

            // Configuration
            container.Register<IConfigurationService, ConfigurationService>(ServiceBehavior.Scoped);

            // Logging
            var logManager = new LoggerManager();
            logManager.AddConnector(new ConsoleConnector());
            logManager.AddConnector(new SerilogConnector());
            container.Register<ILoggerManager>(ServiceBehavior.Scoped, () => logManager);
            container.Register<ILogger, Logger>(ServiceBehavior.Scoped);

            // Commands
            container.Register<JoinCommand>(ServiceBehavior.Transient);
            container.Register<ExitCommand>(ServiceBehavior.Transient);
            container.Register<MoveCommand>(ServiceBehavior.Transient);
            container.Register<ILightFactory<IGameCommand>, CommandFactory>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IServerGameLogic, ServerGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Server>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
