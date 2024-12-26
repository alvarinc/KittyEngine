using KittyEngine.Core.Client.Outputs;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public interface IWPFKeyboardListener : IInputHandler
    {
        void RegisterKeyboardEvents(IGameHost host);
        bool IsEnabled { get; set; }
    }
}
