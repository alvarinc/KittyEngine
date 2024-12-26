using KittyEngine.Core.Client.Commands;
using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging.Conenctors;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Graphics;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Physics;
using KittyEngine.Core.Client.Outputs;

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
                container.Register<IRenderer, Graphics.ConsoleRenderer.ConsoleRenderer>(ServiceBehavior.Scoped);
            }
            else if (clientType == ClientType.WPF)
            {
                // WPF Keyboard
                container.Register<IKeyboadPressedKeyMap, KeyboadPressedKeyMap>(ServiceBehavior.Scoped);
                container.Register<Input.WPFKeyboard.Converters.ExitConverter>(ServiceBehavior.Scoped);
                container.Register<Input.WPFKeyboard.Converters.MoveConverter>(ServiceBehavior.Scoped);
                container.Register<IWPFKeyboardListener, Input.WPFKeyboard.WPFKeyboardListener>(ServiceBehavior.Scoped);

                // WPF Mouse
                container.Register<Input.WPFMouse.IMouseControllerInterop, Input.WPFMouse.MouseControllerInterop>(ServiceBehavior.Scoped);
                container.Register<Input.WPFMouse.IMouseInputFactory, Input.WPFMouse.MouseInputFactory>(ServiceBehavior.Scoped);
                container.Register<Input.WPFMouse.Converters.RotateConverter>(ServiceBehavior.Scoped);
                container.Register<Input.WPFMouse.IWPFMouseListener, Input.WPFMouse.WPFMouseListener>(ServiceBehavior.Scoped);

                // WPF Inputs
                container.Register<IInputHandler, WPFInputHanlder>(ServiceBehavior.Scoped);

                // WPF Output
                container.Register<IOutputFactory, OutputFactory>(ServiceBehavior.Scoped);
                container.Register<IRenderer, Graphics.WPFRenderer.WPFRenderer>(ServiceBehavior.Scoped);
            }

            // Physics
            container.Register<IPrimitiveMoveService, PrimitiveMoveService>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Client>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
