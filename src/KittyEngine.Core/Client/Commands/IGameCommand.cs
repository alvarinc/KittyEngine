using KittyEngine.Core.Server;

namespace KittyEngine.Core.Client.Commands
{
    internal interface IGameCommand
    {
        bool ValidateParameters(GameCommandInput cmd);
        void Execute(GameCommandContext context);
    }
}
