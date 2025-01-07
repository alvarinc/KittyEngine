
namespace KittyEngine.Core.Server.Behaviors.Compositions
{
    public static class CompositionExtensions
    {
        public static List<CompositionBehavior> AddServerBehavior<TServerBehavior>(this List<CompositionBehavior> compositionBehaviors)
            where TServerBehavior : ServerBehavior
        {
            compositionBehaviors.Add(new ServerBehaviorComposer<TServerBehavior>());
            return compositionBehaviors;
        }

        public static List<CompositionBehavior> AddFpsServerBehaviors(this List<CompositionBehavior> compositionBehaviors)
        {
            compositionBehaviors
                .AddServerBehavior<Commands.ExitBehavior>()
                .AddServerBehavior<Commands.JoinBehavior>()
                .AddServerBehavior<Commands.JumpBehavior>()
                .AddServerBehavior<Commands.LoadMapBehavior>()
                .AddServerBehavior<Commands.MoveBehavior>()
                .AddServerBehavior<Commands.RotateBehavior>();

            return compositionBehaviors;
        }
    }
}
