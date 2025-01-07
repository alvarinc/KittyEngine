using KittyEngine.Core.Client.Commands;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Behaviors
{
    public abstract class ClientBehavior
    {
        public virtual void OnStartGame()
        {
        }

        public virtual void OnCommandReceived(GameCommandContext context, GameCommandInput input)
        {
        }

        public virtual GameCommandInput OnMouseEvent(GameState gameState, string playerId, MouseInput input)
        {
            return null;
        }

        public virtual GameCommandInput OnKeyboardEvent(GameState gameState, string playerId, KeyboardInput input)
        {
            return null;
        }

        public virtual void OnStopGame()
        {
        }
    }
}
