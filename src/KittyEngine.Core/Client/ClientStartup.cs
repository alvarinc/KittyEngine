using KittyEngine.Core.Client.Commands;
using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Client.Input.Keyboard;
using KittyEngine.Core.Client.Output;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging.Conenctors;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.Services.Configuration;

namespace KittyEngine.Core.Client
{
    public static class ClientStartup
    {
        public static IServiceContainer ConfigureGameClient(this IServiceContainer container)
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
            container.Register<SynchronizeCommand>(ServiceBehavior.Transient);
            container.Register<ILightFactory<IGameCommand>, CommandFactory>(ServiceBehavior.Scoped);

            // Input / Output
            container.Register<IRenderer, Renderer>(ServiceBehavior.Scoped);
            container.Register<IInputHandler, KeyboardEventHandler>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Client>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
