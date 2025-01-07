
namespace KittyEngine.Core.Client.Behaviors
{
    public interface IClientBehaviorContainer
    {
        void AddBehavior(ClientBehavior behavior);
        void AddBehavior<TBehavior>() where TBehavior : ClientBehavior;

        List<ClientBehavior> GetBehaviors();
    }
}
