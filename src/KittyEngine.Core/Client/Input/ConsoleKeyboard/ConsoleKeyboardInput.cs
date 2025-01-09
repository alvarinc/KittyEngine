
namespace KittyEngine.Core.Client.Input.ConsoleKeyboard
{
    public class ConsoleKeyboardInput
    {
        public ConsoleKeyboardInputType Type { get; set; }
        public ConsoleKey[] PressedKeys { get; set; }
        public ConsoleKey KeyUp { get; set; }
        public ConsoleKey KeyDown { get; set; }
        public bool IsNewKeyDown { get; set; }
    }
}
