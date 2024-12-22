using KittyEngine.Core.Server;

namespace KittyEngine.Core.Client.Commands
{
    internal interface IGameCommand
    {
        public bool ValidateParameters(GameCommandInput cmd);
        public void Execute(GameCommandContext context);
    }
}
