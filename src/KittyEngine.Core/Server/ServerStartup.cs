using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Server
{
    public static class ServerStartup
    {
        public static IServiceContainer ConfigureGameServer(this IServiceContainer container)
        {
            container.Register<ILightFactory<IGameCommand>, CommandFactory>(ServiceBehavior.Scoped);
            container.Register<IServerGameLogic, ServerGameLogic>(ServiceBehavior.Scoped);

            container.Register<Server>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
