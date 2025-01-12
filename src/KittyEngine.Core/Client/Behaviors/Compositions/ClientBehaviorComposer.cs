using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client.Behaviors.Compositions
{
    public class ClientBehaviorComposer<TClientBehavior> : CompositionBehavior where TClientBehavior : ClientBehavior
    {
        public override void OnStartup(IServiceContainer container)
        {
            container.Register<TClientBehavior>(ServiceBehavior.Scoped);
        }

        public override void OnConfigureServices(IServiceContainer container)
        {
            var behavoirContainer = container.Get<IClientBehaviorContainer>();
            behavoirContainer.AddBehavior<TClientBehavior>();
        }
    }
}
