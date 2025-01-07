
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Server.Behaviors
{
    public class ServerBehaviorContainer : IServerBehaviorContainer
    {
        private IServiceContainer _container;
        private List<ServerBehavior> _behaviors = new();
        private List<Type> _types = new();

        public ServerBehaviorContainer(IServiceContainer container)
        {
            _container = container;
        }

        public List<ServerBehavior> GetBehaviors() => _behaviors;

        public void AddBehavior(ServerBehavior behavior)
        {
            var behaviorType = behavior.GetType();

            if (_types.Contains(behaviorType))
            {
                throw new InvalidOperationException($"Behavior already registerer : {behaviorType.FullName}");
            }

            _behaviors.Add(behavior);
            _types.Add(behaviorType);
        }

        public void AddBehavior<TBehavior>() where TBehavior : ServerBehavior
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
