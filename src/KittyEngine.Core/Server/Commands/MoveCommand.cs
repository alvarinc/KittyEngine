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
            var playerState = context.GameState.Players[context.Player.PeerId];
            var results = new GameCommandResult();

            if (_direction.X != 0)
            {
                results = ComputeMove(context, playerState, results, new Vector3D(_direction.X, 0, 0));
            }

            if (_direction.Y != 0)
            {
                results = ComputeMove(context, playerState, results, new Vector3D(0, _direction.Y, 0));
            }

            if (_direction.Z != 0)
            {
                results = ComputeMove(context, playerState, results, new Vector3D(0, 0, _direction.Z));
            }

            if (results.StateUpdated)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} moved to: {playerState.Position}");
            }

            return results;
        }

        private GameCommandResult ComputeMove(GameCommandContext context, PlayerState playerState, GameCommandResult results, Vector3D direction)
        {
            var result = _collisionManager.DetectCollisions(new CollisionDetectionParameters
            {
                MovableBody = playerState,
                MoveDirection = direction,
                BvhTree = context.GameState.BvhTree
            });

            if (!result.HasCollision)
            {
                playerState.Position = playerState.Position + direction;
                results.StateUpdated = true;
            }
            //else if (result.NearestMoveComputed)
            //{
            //    playerState.Position = playerState.Position + result.NearestMove;
            //    results.StateUpdated = true;
            //}

            return results;
        }
    }
}
