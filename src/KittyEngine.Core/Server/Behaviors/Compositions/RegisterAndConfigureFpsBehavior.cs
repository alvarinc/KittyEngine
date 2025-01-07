using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Server.Behaviors.Compositions
{
    internal class RegisterAndConfigureFpsBehavior : CompositionBehavior
    {
        public override void OnStartup(IServiceContainer container)
        {
            container.Register<Commands.ExitBehavior>(ServiceBehavior.Scoped);
            container.Register<Commands.JoinBehavior>(ServiceBehavior.Scoped);
            container.Register<Commands.JumpBehavior>(ServiceBehavior.Scoped);
            container.Register<Commands.LoadMapBehavior>(ServiceBehavior.Scoped);
            container.Register<Commands.MoveBehavior>(ServiceBehavior.Scoped);
            container.Register<Commands.RotateBehavior>(ServiceBehavior.Scoped);
        }

        public override void OnConfigureServices(IServiceContainer container)
        {
            var behavoirContainer = container.Get<IServerBehaviorContainer>();

            behavoirContainer.AddBehavior<Commands.ExitBehavior>();
            behavoirContainer.AddBehavior<Commands.JoinBehavior>();
            behavoirContainer.AddBehavior<Commands.JumpBehavior>();
            behavoirContainer.AddBehavior<Commands.LoadMapBehavior>();
            behavoirContainer.AddBehavior<Commands.MoveBehavior>();
            behavoirContainer.AddBehavior<Commands.RotateBehavior>();
        }
    }
}
