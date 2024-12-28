using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Commands
{
    internal class RotateCommand3D : IGameCommand
    {
        private ILogger _logger;

        private Vector3D _direction;

        public RotateCommand3D(ILogger logger)
        {
            _logger = logger;
        }

        public bool ValidateParameters(GameCommandInput input)
        {
            try
            {
                _direction = Vector3D.Parse(input.Args["direction"]);
                var identity = new Vector3D(0, 0, 0);

                return _direction != identity;
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

            playerState.LookDirection = _direction;
            playerState.LookDirection.Normalize();

            results.StateUpdated = true;

            _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} look to: {playerState.LookDirection}");

            return results;
        }
    }
}
