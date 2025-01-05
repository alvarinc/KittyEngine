using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Commands
{
    internal class JumpCommand : IGameCommand
    {
        private ILogger _logger;
        private ICollisionManager _collisionManager;

        private Vector3D _direction;

        public JumpCommand(ILogger logger, ICollisionManager collisionManager)
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

            var playerState = context.GameState.Players[context.Player.PeerId];
            if (!playerState.IsGrounded)
            {
                return results;
            }

            playerState.Velocity = _direction;
            playerState.VerticalVelocity = 20;
            playerState.IsGrounded = false;

            _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} jump");

            results.StateUpdated = true;
            return results;
        }
    }
}
