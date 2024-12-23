using KittyEngine.Core.Server.Model;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;
using System.Collections.Generic;

namespace KittyEngine.Core.Server.Commands
{
    internal class ExitCommand : GameCommandBase
    {
        private ILogger _logger;

        public ExitCommand(ILogger logger)
        {
            _logger = logger;
        }

        public override bool ValidateParameters(GameCommandInput cmd)
        {
            return true;
        }

        public override GameCommandResult Execute(GameState gameState, Player player)
        {
            _logger.Log(LogLevel.Info, $"[Server] Player {player.PeerId} : {player.Name} requested to stop. Remove from game.");
            gameState.Players.Remove(player.PeerId);

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = false,
            };
        }
    }
}
