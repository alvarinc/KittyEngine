using KittyEngine.Core.Services.Logging;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Commands
{
    internal class MoveCommand : IGameCommand
    {
        private ILogger _logger;

        private Vector3D _direction;

        public MoveCommand(ILogger logger)
        {
            _logger = logger;
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

            //if (_direction.X != 0 && playerState.Position.X + _direction.X >= gameState.Map.MinX && playerState.Position.X + _direction.X <= gameState.Map.MaxX)
            //{
            playerState.Position = playerState.Position + new Vector3D(_direction.X, 0, 0);
                results.StateUpdated = true;
            //}

            //if (_direction.Y != 0 && playerState.Position.Y + _direction.Y >= gameState.Map.MinY && playerState.Position.Y + _direction.Y <= gameState.Map.MaxY)
            //{
                playerState.Position = playerState.Position + new Vector3D(0, _direction.Y, 0);
                results.StateUpdated = true;
            //}

            //if (_direction.Z != 0 && playerState.Position.Z + _direction.Z >= gameState.Map.MinZ && playerState.Position.Z + _direction.Z <= gameState.Map.MaxZ)
            //{
                playerState.Position = playerState.Position + new Vector3D(0, 0, _direction.Z);
                results.StateUpdated = true;
            //}

            if (results.StateUpdated)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {playerState.Name} moved to: {playerState.Position}");
            }

            return results;
        }
    }
}
