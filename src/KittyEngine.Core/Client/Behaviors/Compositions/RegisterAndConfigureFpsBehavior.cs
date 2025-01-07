using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client.Behaviors.Compositions
{
    internal class RegisterAndConfigureFpsBehavior : CompositionBehavior
    {
        public override void OnStartup(IServiceContainer container)
        {
            container.Register<Inputs.ExitBehavior>(ServiceBehavior.Scoped);
            container.Register<Inputs.MoveBehavior>(ServiceBehavior.Scoped);
            container.Register<Inputs.RotateBehavior>(ServiceBehavior.Scoped);
            container.Register<Inputs.JumpBehavior>(ServiceBehavior.Scoped);

            container.Register<Commands.SynchronizeBehavior>(ServiceBehavior.Scoped);
            container.Register<Commands.JoinedBehavior>(ServiceBehavior.Scoped);
        }

        public override void OnConfigureServices(IServiceContainer container)
        {
            var behavoirContainer = container.Get<IClientBehaviorContainer>();

            behavoirContainer.AddBehavior<Inputs.ExitBehavior>();
            behavoirContainer.AddBehavior<Inputs.MoveBehavior>();
            behavoirContainer.AddBehavior<Inputs.RotateBehavior>();
            behavoirContainer.AddBehavior<Inputs.JumpBehavior>();

            behavoirContainer.AddBehavior<Commands.SynchronizeBehavior>();
            behavoirContainer.AddBehavior<Commands.JoinedBehavior>();
        }
    }
}
