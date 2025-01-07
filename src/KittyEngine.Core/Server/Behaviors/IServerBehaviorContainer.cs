
namespace KittyEngine.Core.Server.Behaviors
{
    public interface IServerBehaviorContainer
    {
        void AddBehavior(ServerBehavior behavior);
        void AddBehavior<TBehavior>() where TBehavior : ServerBehavior;

        List<ServerBehavior> GetBehaviors();
    }
}
