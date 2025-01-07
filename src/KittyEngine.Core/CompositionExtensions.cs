namespace KittyEngine.Core
{
    public static class CompositionExtensions
    {
        public static List<CompositionBehavior> AddComposer<TCompositionBehavior>(this List<CompositionBehavior> compositionBehaviors)
            where TCompositionBehavior : CompositionBehavior, new()
        {
            compositionBehaviors.Add(new TCompositionBehavior());
            return compositionBehaviors;
        }

        public static List<CompositionBehavior> AddComposer<TCompositionBehavior>(this List<CompositionBehavior> compositionBehaviors, TCompositionBehavior behavior)
            where TCompositionBehavior : CompositionBehavior
        {
            compositionBehaviors.Add(behavior);
            return compositionBehaviors;
        }
    }
}
