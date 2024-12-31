using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Commands
{
    internal class MoveCommand : IGameCommand
    {
        private ILogger _logger;
        private ICollisionManager _collisionManager;

        private Vector3D _direction;

        public MoveCommand(ILogger logger, ICollisionManager collisionManager)
        {
            _logger = logger;
            _collisionManager = collisionManager;
        }

        public bool ValidateParameters(GameCommandInput input)
        {
            try
            {
                _direction = Vector3D.Parse(input.Args["direction"]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public GameCommandResult Execute(GameCommandContext context)
        {
            var playerState = context.GameState.Players[context.Player.PeerId];
            var results = new GameCommandResult();

            if (_direction.X != 0)
            {
                var directionX = new Vector3D(_direction.X, 0, 0);
                if (!_collisionManager.DetectCollisions(CreateParameters(context, directionX)).HasCollision)
                {
                    playerState.Position = playerState.Position + directionX;
                    results.StateUpdated = true;
                }
            }

            if (_direction.Y != 0)
            {
                var directionY = new Vector3D(0, _direction.Y, 0);
                if (!_collisionManager.DetectCollisions(CreateParameters(context, directionY)).HasCollision)
                {
                    playerState.Position = playerState.Position + directionY;
                    results.StateUpdated = true;
                }
            }

            if (_direction.Z != 0)
            {
                var directionZ = new Vector3D(0, 0, _direction.Z);
                if (!_collisionManager.DetectCollisions(CreateParameters(context, directionZ)).HasCollision)
                {
                    playerState.Position = playerState.Position + directionZ;
                    results.StateUpdated = true;
                }
            }

            if (results.StateUpdated)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} moved to: {playerState.Position}");
            }

            return results;
        }

        private CollisionDetectionParameters CreateParameters(GameCommandContext context, Vector3D direction)
        {
            var playerState = context.GameState.Players[context.Player.PeerId];
            return new CollisionDetectionParameters
            {
                Origin = playerState.Position,
                MoveDirection = direction,
                ObjectBounds = playerState.GetBounds(playerState.Position + direction),
                BvhTree = context.GameState.BvhTree,
                ComputeWallSlidingIfCollid = true
            };
        }
    }
}
