using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Graphics.Renderer;
using KittyEngine.Core.Graphics;
using KittyEngine.Core.Physics;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging.Conenctors;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;
using KittyEngine.Core.Terminal.Renderer;
using System.ComponentModel;

namespace KittyEngine.Core.Client.Behaviors.Compositions
{
    public class RegisterServicesBehavior : CompositionBehavior
    {
        private ClientType _clientType;

        public RegisterServicesBehavior(ClientType clientType)
        {
            _clientType = clientType;
        }

        public override void OnStartup(IServiceContainer container)
        {
            // IoC
            container.Register(ServiceBehavior.Scoped, () => container);

            // Configuration
            container.Register<IConfigurationService, ConfigurationService>(ServiceBehavior.Scoped);

            // Logging
            var logManager = new LoggerManager();
            logManager.AddConnector(new ConsoleConnector());
            logManager.AddConnector(new SerilogConnector());
            container.Register<ILoggerManager>(ServiceBehavior.Scoped, () => logManager);
            container.Register<ILogger, Logger>(ServiceBehavior.Scoped);

            // State
            container.Register<ClientState>(ServiceBehavior.Scoped);

            // Behaviors
            container.Register<IClientBehaviorContainer, ClientBehaviorContainer>(ServiceBehavior.Scoped);

            if (_clientType == ClientType.Desktop)
            {
                OnWPFStartup(container);
            }
            else
            {
                OnConsoleStartup(container);
            }

            // Physics
            container.Register<IPrimitiveMoveService, PrimitiveMoveService>(ServiceBehavior.Scoped);

            // Maps
            container.Register<IContentService, ContentService>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Client>(ServiceBehavior.Scoped);
        }

        private void OnConsoleStartup(IServiceContainer container)
        {
            // Console Input
            container.Register<IInputHandler, ConsoleInputHanlder>(ServiceBehavior.Scoped);

            // Console Output
            container.Register<IRenderer, TerminalRenderer>(ServiceBehavior.Scoped);
        }

        private void OnWPFStartup(IServiceContainer container)
        {
            // WPF Inputs
            container.Register<IInputHandler, WPFInputHanlder>(ServiceBehavior.Scoped);

            // WPF Renderer
            container.Register<IImageAssetProvider, ImageAssetProvider>(ServiceBehavior.Scoped);
            container.Register<ILayeredModel3DFactory, LayeredModel3DFactory>(ServiceBehavior.Scoped);
            container.Register<IGameWorldBuilder, GameWorldBuilder>(ServiceBehavior.Scoped);
            container.Register<IGameWorldRenderer, GameWorldRenderer>(ServiceBehavior.Scoped);

            // WPF Output
            container.Register<IOutputFactory, OutputFactory>(ServiceBehavior.Scoped);
            container.Register<IRenderer, WPFRenderer>(ServiceBehavior.Scoped);

            // WPF Host
            container.Register<IGameHost, GameHost>(ServiceBehavior.Scoped);
        }
    }
}
