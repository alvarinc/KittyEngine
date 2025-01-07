using KittyEngine.Core.Server.Commands;
using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Behaviors.Commands
{
    public class JumpBehavior : ServerBehavior
    {
        private ILogger _logger;

        public JumpBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public override GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            var results = new GameCommandResult();

            if (input.Command != "jump")
            {
                return results;
            }

            Vector3D direction;

            try
            {
                direction = Vector3D.Parse(input.Args["direction"]);
            }
            catch
            {
                return results;
            }

            var playerState = context.GameState.Players[context.Player.PeerId];
            if (!playerState.IsGrounded)
            {
                return results;
            }

            playerState.Velocity = direction;
            playerState.VerticalVelocity = 20;
            playerState.IsGrounded = false;

            _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} jump");

            results.StateUpdated = true;
            return results;
        }
    }
}
