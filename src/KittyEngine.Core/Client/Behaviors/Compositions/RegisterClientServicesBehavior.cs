﻿using KittyEngine.Core.Client.Commands;
using KittyEngine.Core.Client.Input.WPFKeyboard;
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

namespace KittyEngine.Core.Client.Behaviors.Compositions
{
    public class RegisterClientServicesBehavior : CompositionBehavior
    {
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

            // Commands
            container.Register<JoinedCommand>(ServiceBehavior.Transient);
            container.Register<SynchronizeCommand>(ServiceBehavior.Transient);
            container.Register<ILightFactory<IGameCommand>, CommandFactory>(ServiceBehavior.Scoped);

            OnWPFStartup(container);

            // Physics
            container.Register<IPrimitiveMoveService, PrimitiveMoveService>(ServiceBehavior.Scoped);

            // Maps
            container.Register<IContentService, ContentService>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Client>(ServiceBehavior.Scoped);
        }

        private void OnWPFStartup(IServiceContainer container)
        {
            // WPF Keyboard
            container.Register<IKeyboadPressedKeyMap, KeyboadPressedKeyMap>(ServiceBehavior.Scoped);
            container.Register<Input.WPFKeyboard.Converters.ExitConverter>(ServiceBehavior.Scoped);
            container.Register<Input.WPFKeyboard.Converters.MoveConverter>(ServiceBehavior.Scoped);
            container.Register<Input.WPFKeyboard.Converters.JumpConverter>(ServiceBehavior.Scoped);
            container.Register<IWPFKeyboardListener, WPFKeyboardListener>(ServiceBehavior.Scoped);

            // WPF Mouse
            container.Register<Input.WPFMouse.IMouseControllerInterop, Input.WPFMouse.MouseControllerInterop>(ServiceBehavior.Scoped);
            container.Register<Input.WPFMouse.IMouseInputFactory, Input.WPFMouse.MouseInputFactory>(ServiceBehavior.Scoped);
            container.Register<Input.WPFMouse.Converters.RotateConverter>(ServiceBehavior.Scoped);
            container.Register<Input.WPFMouse.IWPFMouseListener, Input.WPFMouse.WPFMouseListener>(ServiceBehavior.Scoped);

            // WPF Inputs
            container.Register<IInputHandler, WPFInputHanlder>(ServiceBehavior.Scoped);

            // WPF Renderer
            container.Register<IImageAssetProvider, ImageAssetProvider>(ServiceBehavior.Scoped);
            container.Register<ILayeredModel3DFactory, LayeredModel3DFactory>(ServiceBehavior.Scoped);
            container.Register<IMapBuilder, MapBuilder>(ServiceBehavior.Scoped);
            container.Register<IMapRenderer, MapRenderer>(ServiceBehavior.Scoped);

            // WPF Output
            container.Register<IOutputFactory, OutputFactory>(ServiceBehavior.Scoped);
            container.Register<IRenderer, WPFRenderer>(ServiceBehavior.Scoped);
        }
    }
}
