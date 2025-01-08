using KittyEngine.Core.Client.Behaviors;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Server;
using KittyEngine.Core.State;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard
{
    public class ConsoleKeyboardListener : IInputHandler
    {
        private ConsoleKeyboardController _keyboardController = new ConsoleKeyboardController();
        private ConsoleKeyToWindowsKeyConverter _converter = new ConsoleKeyToWindowsKeyConverter();
        private IClientBehaviorContainer _behaviorContainer;

        public ConsoleKeyboardListener(IClientBehaviorContainer behaviorContainer)
        {
            _behaviorContainer = behaviorContainer;
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var results = new List<GameCommandInput>();
            var inputs = new List<KeyboardInput>();

            _keyboardController.HandleInputs();

            var pressedKeys = _converter.Convert(_keyboardController.GetPressedKeys());

            foreach (var key in pressedKeys)
            {
                if (_keyboardController.IsKeyDown(_converter.Convert(key).Value))
                {
                    inputs.Add(new KeyboardInput
                    {
                        Type = KeyboardInputType.KeyDown,
                        KeyDown = key,
                        IsNewKeyDown = true,
                        KeyUp = Key.None,
                        PressedKeys = pressedKeys.ToArray()
                    });
                }
                else if (_keyboardController.IsKeyUp(_converter.Convert(key).Value))
                {
                    inputs.Add(new KeyboardInput
                    {
                        Type = KeyboardInputType.KeyDown,
                        KeyDown = Key.None,
                        IsNewKeyDown = false,
                        KeyUp = key,
                        PressedKeys = pressedKeys.ToArray()
                    });
                }
            }

            // If no keyboard inputs, and still have pressed keys, send the PressedKeys map
            if (inputs.Count == 0)
            {
                if (pressedKeys.Count > 0)
                {
                    inputs.Add(new KeyboardInput
                    {
                        Type = KeyboardInputType.KeyPressedMap,
                        PressedKeys = pressedKeys.ToArray()
                    });
                }
            }

            // Send inputs to behaviors
            var clientBehaviors = _behaviorContainer.GetBehaviors();
            foreach (var keyboardInput in inputs)
            {
                foreach (var clientBehavior in clientBehaviors)
                {
                    var cmd = clientBehavior.OnKeyboardEvent(gameState, playerId, keyboardInput);

                    if (cmd != null)
                    {
                        results.Add(cmd);
                    }
                }
            }

            return results;
        }
    }
}
