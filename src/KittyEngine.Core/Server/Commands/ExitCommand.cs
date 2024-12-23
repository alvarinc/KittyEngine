using KittyEngine.Core.Server.Model;
using KittyEngine.Core.State;
using System.Collections.Generic;

namespace KittyEngine.Core.Server.Commands
{
    internal class ExitCommand : GameCommandBase
    {
        public override bool ValidateParameters(GameCommandInput cmd)
        {
            return true;
        }

        public override GameCommandResult Execute(GameState gameState, Player player)
        {
            Console.WriteLine($"[Server] Player {player.PeerId} : {player.Name} requested to stop. Remove from game.");
            gameState.Players.Remove(player.PeerId);

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = false,
            };
        }
    }
}
