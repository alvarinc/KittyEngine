using KittyEngine.Core.State;

namespace KittyEngine.Core.Server.Commands
{
    internal class NopCommand : GameCommandBase
    {
        public override bool ValidateParameters(GameCommandInput input)
        {
            return true;
        }

        public override GameCommandResult Execute(GameState gameState, Player player, int peerId)
        {
            return new GameCommandResult();
        }
    }
}
