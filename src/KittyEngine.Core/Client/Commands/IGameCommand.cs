using KittyEngine.Core.Server;

namespace KittyEngine.Core.Client.Commands
{
    internal interface IGameCommand
    {
        bool ValidateParameters(GameCommandInput input);
        void Execute(GameCommandContext context);
    }
}
