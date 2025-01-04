using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;
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

            if (_direction.X == 0 && _direction.Y == 0 && _direction.Z == 0)
            {
                return results;
            }

            var playerState = context.GameState.Players[context.Player.PeerId];

            results = ComputeMove(context, playerState, results, _direction);

            if (results.StateUpdated)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} moved to: {playerState.Position}");
            }

            return results;
        }

        private GameCommandResult ComputeMove(GameCommandContext context, PlayerState playerState, GameCommandResult results, Vector3D direction)
        {
            var playerMoved = false;
            var collisionParameters = new CollisionDetectionParameters
            {
                MovableBody = playerState,
                MoveDirection = direction,
                MapBvhTree = context.GameState.MapBvhTree
            };

            var collistionResult = _collisionManager.DetectCollisions(collisionParameters);

            if (!collistionResult.HasCollision)
            {
                playerState.Position = playerState.Position + direction;
                playerMoved = true;
            }

            if (!playerMoved)
            {
                var stairClimbingResult = _collisionManager.ComputeStairClimbing(collisionParameters, collistionResult, _logger);

                if (stairClimbingResult.CanClimbStairs)
                {
                    playerState.Position = playerState.Position + stairClimbingResult.Direction;
                    playerMoved = true;
                }
            }

            if (!playerMoved)
            {
                var wallSlidingResult = _collisionManager.ComputeWallSliding(collisionParameters, collistionResult);
                if (wallSlidingResult.CanWallSlide)
                {
                    playerState.Position = playerState.Position + wallSlidingResult.Direction;
                    playerMoved = true;
                }
            }

            results.StateUpdated = playerMoved;
            return results;
        }
    }
}
