using System.Windows.Controls;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public interface IWPFKeyboardListener : IInputHandler
    {
        void RegisterKeyboardEvents(UserControl control);
        bool IsEnabled { get; set; }
    }
}
