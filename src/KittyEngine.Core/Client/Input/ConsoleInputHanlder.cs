using KittyEngine.Core.Client.Input.ConsoleKeyboard;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    internal class ConsoleInputHanlder : IInputHandler
    {
        private ConsoleKeyboardListener _keyboardListener;
        public ConsoleInputHanlder(ConsoleKeyboardListener keyboardListener)
        {
            _keyboardListener = keyboardListener;
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            return _keyboardListener.HandleEvents(gameState, playerId);
        }
    }
}
