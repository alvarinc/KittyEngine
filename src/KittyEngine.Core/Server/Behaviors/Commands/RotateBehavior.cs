using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Behaviors.Commands
{
    internal class RotateBehavior : ServerBehavior
    {
        private ILogger _logger;

        public RotateBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public override GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "rotate3d")
            {
                return GameCommandResult.None;
            }

            Vector3D _direction;
            try
            {
                _direction = Vector3D.Parse(input.Args["direction"]);

                if (_direction == new Vector3D(0, 0, 0))
                {
                    return GameCommandResult.None;
                }
            }
            catch
            {
                return GameCommandResult.None;
            }

            var playerState = context.GameState.Players[context.Player.PeerId];

            playerState.LookDirection = _direction;
            playerState.LookDirection.Normalize();

            _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} look to: {playerState.LookDirection}");

            return new GameCommandResult { StateUpdated = true };
        }
    }
}
