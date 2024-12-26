using System.Windows;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public interface IMouseControllerInterop
    {
        Point CenterCursorPosition(FrameworkElement element);
    }
}
