
namespace KittyEngine.Core.Client.Input.ConsoleKeyboard
{
    public class ConsoleKeyboardListener
    {
        private ConsoleKeyboardController _keyboardController = new ConsoleKeyboardController();

        public void HandleEvents(Action<ConsoleKeyboardInput> handler)
        {
            _keyboardController.HandleInputs();

            var pressedKeys = _keyboardController.GetPressedKeys();
            var inputs = new List<ConsoleKeyboardInput>();

            foreach (var key in pressedKeys)
            {
                if (_keyboardController.IsKeyDown(key))
                {
                    inputs.Add(new ConsoleKeyboardInput
                    {
                        Type = ConsoleKeyboardInputType.KeyDown,
                        KeyDown = key,
                        IsNewKeyDown = true,
                        KeyUp = ConsoleKey.None,
                        PressedKeys = pressedKeys.ToArray()
                    });
                }
                else if (_keyboardController.IsKeyUp(key))
                {
                    inputs.Add(new ConsoleKeyboardInput
                    {
                        Type = ConsoleKeyboardInputType.KeyDown,
                        KeyDown = ConsoleKey.None,
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
                    inputs.Add(new ConsoleKeyboardInput
                    {
                        Type = ConsoleKeyboardInputType.KeyPressedMap,
                        PressedKeys = pressedKeys.ToArray()
                    });
                }
            }

            // If no handler, still catch events and do nothing
            if (handler == null)
            {
                return;
            }

            // Send inputs to behaviors
            foreach (var keyboardInput in inputs)
            {
                handler(keyboardInput);
            }
        }
    }
}
