using KittyEngine.Core.Server.Model;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal class JoinCommand : GameCommandBase
    {
        private string _guid;
        private string _name;

        public override bool ValidateParameters(GameCommandInput input)
        {
            _guid = input.Args["guid"];
            _name = input.Args["name"];
            return true;
        }

        public override GameCommandResult Execute(GameState gameState, Player player)
        {
            player.Guid = _guid;
            player.Name = _name;

            gameState.Players.Add(player.PeerId, new PlayerState(player.PeerId));
            gameState.Players[player.PeerId].Name = player.Name;
            gameState.Players[player.PeerId].Guid = player.Guid;

            Console.WriteLine($"[Server] Player {player.PeerId} : {player.Name} joined the game");

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = true,
            };
        }
    }
}
