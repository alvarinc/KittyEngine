using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public class KeyboardInput
    {
        public KeyboardInputType Type { get; set; }
        public Key[] PressedKeys { get; set; }
        public Key KeyUp { get; set; }
        public Key KeyDown { get; set; }
        public bool IsNewKeyDown { get; set; }
    }
}
