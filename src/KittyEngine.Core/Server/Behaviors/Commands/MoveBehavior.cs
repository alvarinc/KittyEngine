using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Behaviors.Commands
{
    public class MoveBehavior : ServerBehavior
    {
        private ILogger _logger;
        private ICollisionManager _collisionManager;

        public MoveBehavior(ILogger logger, ICollisionManager collisionManager)
        {
            _logger = logger;
            _collisionManager = collisionManager;
        }

        public override GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "move" && input.Command != "move3d")
            {
                return GameCommandResult.None;
            }

            Vector3D direction;

            try
            {
                direction = Vector3D.Parse(input.Args["direction"]);
            }
            catch
            {
                return GameCommandResult.None;
            }

            if (direction == new Vector3D(0, 0, 0))
            {
                return GameCommandResult.None;
            }

            var playerState = context.GameState.Players[context.Player.PeerId];
            if (!playerState.IsGrounded)
            {
                return GameCommandResult.None;
            }

            var playerMoved = false;
            var collisionParameters = new CollisionDetectionParameters
            {
                RigidBody = playerState,
                MoveDirection = direction,
                MapBvhTree = context.GameState.MapBvhTree
            };

            var collisionResult = _collisionManager.DetectCollisions(collisionParameters);

            if (!collisionResult.HasCollision)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : Move");
                playerState.Position = playerState.Position + direction;
                playerState.Velocity = direction;
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

            return new GameCommandResult { StateUpdated = playerMoved };
        }
    }
}
