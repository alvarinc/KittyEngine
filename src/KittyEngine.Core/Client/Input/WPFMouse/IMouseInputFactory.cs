using System.Windows.Controls;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public interface IMouseInputFactory
    {
        MouseInput CreateMouseButtonInput(Viewport3D viewport, MouseButtonEventArgs e);
        MouseInput CreateMouseMoveInput(Viewport3D viewport, MouseEventArgs e);
        MouseInput CreateMouseWheelInput(Viewport3D viewport, MouseWheelEventArgs e);
        void Reset();
    }
}
