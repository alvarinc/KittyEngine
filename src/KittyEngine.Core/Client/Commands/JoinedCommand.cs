using KittyEngine.Core.Server;
using KittyEngine.Core.Server.Model;

namespace KittyEngine.Core.Client.Commands
{
    internal class JoinedCommand : IGameCommand
    {
        private string _guid;
        private string _name;

        public bool ValidateParameters(GameCommandInput input)
        {
            _guid = input.Args["guid"];
            _name = input.Args["name"];
            return true;
        }

        public void Execute(GameCommandContext context)
        {
            var player = new Player(context.PeerId);
            player.Guid = _guid;
            player.Name = _name;
            
            context.State.ConnectedUser = player;
            context.StateUpdated = true;
        }
    }
}
