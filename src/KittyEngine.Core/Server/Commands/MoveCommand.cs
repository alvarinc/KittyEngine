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
            var results = new GameCommandResult();

            if (_direction == new Vector3D(0, 0, 0))
            {
                return results;
            }

            var playerState = context.GameState.Players[context.Player.PeerId];
            if (!playerState.IsGrounded)
            {
                return results;
            }

            var playerMoved = false;
            var collisionParameters = new CollisionDetectionParameters
            {
                RigidBody = playerState,
                MoveDirection = _direction,
                MapBvhTree = context.GameState.MapBvhTree
            };

            var collisionResult = _collisionManager.DetectCollisions(collisionParameters);

            if (!collisionResult.HasCollision)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : Move");
                playerState.Position = playerState.Position + _direction;
                playerState.Velocity = _direction;
                playerMoved = true;
            }

            if (!playerMoved)
            {
                var stairClimbingResult = _collisionManager.ComputeStairClimbing(collisionParameters, collisionResult);

                if (stairClimbingResult.CanClimbStairs)
                {
                    _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : ClimbStairs");
                    playerState.Position = playerState.Position + stairClimbingResult.Direction;
                    playerState.Velocity = stairClimbingResult.Direction;
                    playerMoved = true;
                }
            }

            if (!playerMoved)
            {
                var wallSlidingResult = _collisionManager.ComputeWallSliding(collisionParameters, collisionResult);
                if (wallSlidingResult.CanWallSlide)
                {
                    _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : WallSliding");
                    playerState.Position = playerState.Position + wallSlidingResult.Direction;
                    playerState.Velocity = wallSlidingResult.Direction;
                    playerMoved = true;
                }
            }

            if (playerMoved)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} moved to: {playerState.Position}");
            }

            results.StateUpdated = playerMoved;
            return results;
        }
    }
}
