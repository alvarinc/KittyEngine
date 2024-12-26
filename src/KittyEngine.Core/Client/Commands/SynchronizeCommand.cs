using KittyEngine.Core.Server;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace KittyEngine.Core.Client.Commands
{
    internal class SynchronizeCommand : IGameCommand
    {
        private ILogger _logger;

        private string _entity;
        private string _mode;
        private string _value;

        public SynchronizeCommand(ILogger logger)
        {
            _logger = logger;
        }

        public bool ValidateParameters(GameCommandInput cmd)
        {
            _entity = cmd.Args["entity"];
            _mode = cmd.Args["mode"];
            _value = cmd.Args["value"];

            return true;
        }

        public void Execute(GameCommandContext context)
        {
            if (_mode == "full")
            {
                context.GameState = JsonConvert.DeserializeObject<GameState>(_value);
                context.StateUpdated = true;
            }
            else if (_mode == "patch")
            {
                var currentPlayer = context.GameState.GetPlayer(context.PlayerId);
                var lookDirection = currentPlayer.LookDirection;

                var jsonPatch = JsonConvert.DeserializeObject<JsonPatchDocument>(_value);
                jsonPatch.ApplyTo(context.GameState);

                currentPlayer.LookDirection = lookDirection;

                context.StateUpdated = true;
            }

            var player = context.GameState?.Players.Values.FirstOrDefault(x => x.Guid == context.PlayerId);
            if (player != null)
            {
                _logger.Log(LogLevel.Info, $"[Client] {player.PeerId} ({player.Name}) : Position: {player.Position.X}, {player.Position.Y}, {player.Position.Z}");
            }
            else
            {
                _logger.Log(LogLevel.Info, $"[Client] Connected. No position yet.");
            }
        }
    }
}
