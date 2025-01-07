using KittyEngine.Core.Server.Commands;

namespace KittyEngine.Core.Server.Behaviors
{
    public class ServerBehavior
    {
        public virtual void OnStartGame()
        {
        }

        public virtual GameCommandResult OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
            return new GameCommandResult();
        }

        public virtual void OnStopGame()
        {
        }
    }
}
