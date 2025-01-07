using KittyEngine.Core.Client.Behaviors;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Physics;
using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.Services.Logging.Conenctors;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Behaviors.Compositions
{
    public class RegisterServicesBehavior : CompositionBehavior
    {
        public override void OnStartup(IServiceContainer container)
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

            // Physics
            container.Register<IPhysicsEngine, PhysicsEngine>(ServiceBehavior.Scoped);

            // State
            container.Register<ServerState>(ServiceBehavior.Scoped);

            // Behaviors
            container.Register<IServerBehaviorContainer, ServerBehaviorContainer>(ServiceBehavior.Scoped);

            // Maps
            container.Register<IContentService, ContentService>(ServiceBehavior.Scoped);
            container.Register<IMapBuilderFactory, MapBuilderFactory>(ServiceBehavior.Scoped);

            // Game logic
            container.Register<IServerGameLogic, ServerGameLogic>(ServiceBehavior.Scoped);
            container.Register<IImageAssetProvider, ImageAssetProvider>(ServiceBehavior.Scoped);
            container.Register<ILayeredModel3DFactory, LayeredModel3DFactory>(ServiceBehavior.Scoped);
            container.Register<ICollisionManager, CollisionManager>(ServiceBehavior.Scoped);

            // Entry point
            container.Register<Server>(ServiceBehavior.Scoped);
        }
    }
}
