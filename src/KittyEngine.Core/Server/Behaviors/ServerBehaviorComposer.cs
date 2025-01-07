using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Server.Behaviors
{
    public class ServerBehaviorComposer<TServerBehavior> : CompositionBehavior where TServerBehavior : ServerBehavior
    {
        public override void OnStartup(IServiceContainer container)
        {
            container.Register<TServerBehavior>(ServiceBehavior.Scoped);
        }

        public override void OnConfigureServices(IServiceContainer container)
        {
            var behavoirContainer = container.Get<IServerBehaviorContainer>();
            behavoirContainer.AddBehavior<TServerBehavior>();
        }
    }
}
