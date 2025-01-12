
namespace KittyEngine.Core.Client.Behaviors.Compositions
{
    public static class CompositionExtensions
    {
        public static List<CompositionBehavior> AddClientBehavior(this List<CompositionBehavior> compositionBehaviors, CompositionBehavior compositionBehavior)
        {
            compositionBehaviors.Add(compositionBehavior);
            return compositionBehaviors;

        }
        public static List<CompositionBehavior> AddClientBehavior<TClientBehavior>(this List<CompositionBehavior> compositionBehaviors)
            where TClientBehavior : ClientBehavior
        {
            compositionBehaviors.Add(new ClientBehaviorComposer<TClientBehavior>());
            return compositionBehaviors;
        }

        public static List<CompositionBehavior> AddFpsClientBehaviors(this List<CompositionBehavior> compositionBehaviors)
        {
            return compositionBehaviors
                .AddClientBehavior<Inputs.ExitBehavior>()
                .AddClientBehavior<Inputs.JumpBehavior>()
                .AddClientBehavior<Inputs.MoveBehavior>()
                .AddClientBehavior<Inputs.RotateBehavior>()

                .AddClientBehavior<Commands.JoinedBehavior>()
                .AddClientBehavior<Commands.SynchronizeBehavior>();
        }
    }
}
