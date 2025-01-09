using KittyEngine.Core.Client.Behaviors;
using KittyEngine.Core.Client.Input.ConsoleKeyboard;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input
{
    internal class ConsoleInputHanlder : IInputHandler
    {
        private IClientBehaviorContainer _behaviorContainer;
        private ConsoleKeyboardListener _keyboardListener;
        private ConsoleKeyToWindowsKeyConverter _converter;

        public ConsoleInputHanlder(IClientBehaviorContainer behaviorContainer)
        {
            _behaviorContainer = behaviorContainer;
            _keyboardListener = new ConsoleKeyboardListener();
            _converter = new ConsoleKeyToWindowsKeyConverter();
        }

        public bool IsEnabled => _keyboardListener.IsEnabled;

        public void Disable()
        {
            _keyboardListener.Disable();
        }

        public void Enable()
        {
            _keyboardListener.Enable();
        }

        public void Reset()
        {
            _keyboardListener.Reset();
        }

        public void RegisterEvents(object gameHost)
        {

        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var results = new List<GameCommandInput>();
            var clientBehaviors = _behaviorContainer.GetBehaviors();

            _keyboardListener.HandleEvents(input =>
            {
                // Convert in order to have a unified clientBehavior for input events for Console and WPF
                var keyboardInput = Convert(input);
                var commands = clientBehaviors
                    .Select(behavior => behavior.OnKeyboardEvent(gameState, playerId, keyboardInput))
                    .Where(command => command != null);

                results.AddRange(commands);
            });

            return results;
        }

        private KeyboardInput Convert(ConsoleKeyboardInput consoleInput)
        {
            var type = consoleInput.Type switch
            {
                ConsoleKeyboardInputType.KeyUp => KeyboardInputType.KeyUp,
                ConsoleKeyboardInputType.KeyDown => KeyboardInputType.KeyDown,
                ConsoleKeyboardInputType.KeyPressedMap => KeyboardInputType.KeyPressedMap,
                _ => throw new NotImplementedException($"keyboard event not managed: {consoleInput.Type}")
            };

            return new KeyboardInput
            {
                Type = type,
                IsNewKeyDown = consoleInput.IsNewKeyDown,
                KeyDown = _converter.Convert(consoleInput.KeyDown) ?? System.Windows.Input.Key.None,
                KeyUp = _converter.Convert(consoleInput.KeyUp) ?? System.Windows.Input.Key.None,
                PressedKeys = _converter.Convert(consoleInput.PressedKeys).ToArray()
            };
        }
    }
}
