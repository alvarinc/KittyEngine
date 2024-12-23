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

        public override GameCommandResult Execute(GameState gameState, Player player, int peerId)
        {
            player.Guid = _guid;
            player.Name = _name;

            gameState.Players.Add(peerId, new PlayerState(peerId));
            gameState.Players[peerId].Name = player.Name;
            gameState.Players[peerId].Guid = player.Guid;

            Console.WriteLine($"[Server] Player {peerId} : {player.Name} has joined the game");

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = true,
            };
        }
    }
}
