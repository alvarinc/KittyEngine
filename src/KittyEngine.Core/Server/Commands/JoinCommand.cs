using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal class JoinCommand : IGameCommand
    {
        private ILogger _logger;

        private string _guid;
        private string _name;

        public JoinCommand(ILogger logger)
        {
            _logger = logger;
        }

        public bool ValidateParameters(GameCommandInput input)
        {
            _guid = input.Args["guid"];
            _name = input.Args["name"];
            return true;
        }

        public GameCommandResult Execute(GameCommandContext context)
        {
            context.Player.Guid = _guid;
            context.Player.Name = _name;

            context.GameState.Players.Add(context.Player.PeerId, new PlayerState(context.Player.PeerId));
            context.GameState.Players[context.Player.PeerId].Name = context.Player.Name;
            context.GameState.Players[context.Player.PeerId].Guid = context.Player.Guid;

            _logger.Log(LogLevel.Info, $"[Server] Player {context.Player.PeerId} : {context.Player.Name} joined the game");

            var command = new GameCommandInput("joined")
              .AddArgument("guid", context.Player.Guid)
              .AddArgument("name", context.Player.Name);

            context.SendMessage(context.Player.PeerId, command);

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = true,
            };
        }
    }
}
