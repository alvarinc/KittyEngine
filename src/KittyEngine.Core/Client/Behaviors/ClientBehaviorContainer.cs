
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client.Behaviors
{
    public class ClientBehaviorContainer : IClientBehaviorContainer
    {
        private IServiceContainer _container;
        private List<ClientBehavior> _behaviors = new();
        private List<Type> _types = new();

        public ClientBehaviorContainer(IServiceContainer container)
        {
            _container = container;
        }

        public List<ClientBehavior> GetBehaviors() => _behaviors;

        public void AddBehavior(ClientBehavior behavior)
        {
            var behaviorType = behavior.GetType();

            if (_types.Contains(behaviorType))
            {
                throw new InvalidOperationException($"Behavior already registerer : {behaviorType.FullName}");
            }

            _behaviors.Add(behavior);
            _types.Add(behaviorType);
        }

        public void AddBehavior<TBehavior>() where TBehavior : ClientBehavior
        {
            var type = typeof(TBehavior);

            if (_types.Contains(type))
            {
                throw new InvalidOperationException($"Behavior already registerer : {type.FullName}");
            }

            _types.Add(type);
            var behavior = _container.Get<TBehavior>();

            _behaviors.Add(behavior);
        }
    }
}
