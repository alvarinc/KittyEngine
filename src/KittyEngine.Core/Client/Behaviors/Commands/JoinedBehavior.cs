﻿using KittyEngine.Core.Server;
using KittyEngine.Core.Server.Model;

namespace KittyEngine.Core.Client.Behaviors.Commands
{
    internal class JoinedBehavior : ClientBehavior
    {
        public override void OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            if (input.Command != "joined")
            {
                return;
            }

            var guid = input.Args["guid"];
            var name = input.Args["name"];

            var player = new Player(context.PeerId);
            player.Guid = guid;
            player.Name = name;

            context.State.Mode = State.ClientMode.InGame;
            context.State.ConnectedUser = player;
            context.StateUpdated = true;
        }
    }
}