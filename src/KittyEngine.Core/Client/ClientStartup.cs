using KittyEngine.Core.Client.Commands;
using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging.Conenctors;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Graphics.ConsoleRenderer;
using KittyEngine.Core.Graphics;
using KittyEngine.Core.Client.Input.WPFKeyboard;

namespace KittyEngine.Core.Client
{
    public static class ClientStartup
    {
        public static IServiceContainer ConfigureGameClient(this IServiceContainer container, ClientType clientType)
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
            container.Register<ILightFactory<Core.Client.Commands.IGameCommand>, Core.Client.Commands.CommandFactory>(ServiceBehavior.Scoped);

            if (clientType == ClientType.Console)
            {
                // Console Input
                container.Register<Input.ConsoleKeyboard.Converters.ExitConverter>(ServiceBehavior.Scoped);
                container.Register<Input.ConsoleKeyboard.Converters.MoveConverter>(ServiceBehavior.Scoped);
                container.Register<Input.ConsoleKeyboard.ConsoleKeyboardListener>(ServiceBehavior.Scoped);
                container.Register<IInputHandler, ConsoleInputHanlder>(ServiceBehavior.Scoped);

                // Console Output
                container.Register<IRenderer, ConsoleRenderer>(ServiceBehavior.Scoped);
            }
            else if (clientType == ClientType.WPF)
            {
                // WPF Input
                container.Register<IKeyboadPressedKeyMap, KeyboadPressedKeyMap>(ServiceBehavior.Scoped);
                container.Register<Input.WPFKeyboard.Converters.ExitConverter>(ServiceBehavior.Scoped);
                container.Register<Input.WPFKeyboard.Converters.MoveConverter>(ServiceBehavior.Scoped);
                container.Register<IWPFKeyboardListener, Input.WPFKeyboard.WPFKeyboardListener>(ServiceBehavior.Scoped);
                container.Register<IInputHandler, WPFInputHanlder>(ServiceBehavior.Scoped);

                // Console Output
                container.Register<IRenderer, ConsoleRenderer>(ServiceBehavior.Scoped);
            }

            // Game logic
            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Client>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
