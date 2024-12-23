using KittyEngine.Core.Client.Commands;
using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Client.Input.Keyboard;
using KittyEngine.Core.Client.Output;
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client
{
    public static class ClientStartup
    {
        public static IServiceContainer ConfigureGameClient(this IServiceContainer container)
        {
            container.Register<ILightFactory<IGameCommand>, CommandFactory>(ServiceBehavior.Scoped);
            container.Register<IRenderer, Renderer>(ServiceBehavior.Scoped);
            container.Register<IInputHandler, KeyboardEventHandler>(ServiceBehavior.Scoped);

            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            container.Register<Client>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
