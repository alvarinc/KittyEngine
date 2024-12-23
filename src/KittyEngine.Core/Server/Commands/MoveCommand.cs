﻿using KittyEngine.Core.Server.Model;
using KittyEngine.Core.Services.Logging;
using KittyEngine.Core.State;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Server.Commands
{
    internal class MoveCommand : GameCommandBase
    {
        private ILogger _logger;

        private float _dx;
        private float _dy;
        private float _dz;

        public MoveCommand(ILogger logger)
        {
            _logger = logger;
        }

        public override bool ValidateParameters(GameCommandInput input)
        {
            var dxString = input.Args["dx"];
            var dyString = input.Args["dy"];
            var dzString = input.Args["dz"];

            return
                float.TryParse(dxString, out _dx) &&
                float.TryParse(dyString, out _dy) &&
                float.TryParse(dzString, out _dz);
        }

        public override GameCommandResult Execute(GameState gameState, Player player)
        {
            var playerState = gameState.Players[player.PeerId];
            var results = new GameCommandResult();
            if (_dx != 0 && playerState.Position.X + _dx >= gameState.Map.MinX && playerState.Position.X + _dx <= gameState.Map.MaxX)
            {
                playerState.Position = playerState.Position + new Vector3D(_dx, 0, 0);
                results.StateUpdated = true;
            }

            if (_dy != 0 && playerState.Position.Y + _dy >= gameState.Map.MinY && playerState.Position.Y + _dy <= gameState.Map.MaxY)
            {
                playerState.Position = playerState.Position + new Vector3D(0, _dy, 0);
                results.StateUpdated = true;
            }

            if (_dz != 0 && playerState.Position.Z + _dz >= gameState.Map.MinZ && playerState.Position.Z + _dz <= gameState.Map.MaxZ)
            {
                playerState.Position = playerState.Position + new Vector3D(0, 0, _dz);
                results.StateUpdated = true;
            }

            if (results.StateUpdated)
            {
                _logger.Log(LogLevel.Info, $"[Server] Player {player.PeerId} : {playerState.Name} moved to: {playerState.Position}");
            }

            return results;
        }
    }
}
