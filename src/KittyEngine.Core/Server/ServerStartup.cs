using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.Services.Logging.Conenctors;
using KittyEngine.Core.State;

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

            // State
            container.Register<ServerState>(ServiceBehavior.Scoped);

            // Commands
            container.Register<LoadMapCommand>(ServiceBehavior.Transient);

            container.Register<JoinCommand>(ServiceBehavior.Transient);
            container.Register<ExitCommand>(ServiceBehavior.Transient);
            container.Register<MoveCommand>(ServiceBehavior.Transient);
            container.Register<MoveCommand3D>(ServiceBehavior.Transient);
            container.Register<RotateCommand3D>(ServiceBehavior.Transient);
            container.Register<ILightFactory<IGameCommand>, CommandFactory>(ServiceBehavior.Scoped);

            // Maps
            container.Register<IContentService, ContentService>(ServiceBehavior.Scoped);
            container.Register<IMapBuilderFactory, MapBuilderFactory>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IServerGameLogic, ServerGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Server>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
