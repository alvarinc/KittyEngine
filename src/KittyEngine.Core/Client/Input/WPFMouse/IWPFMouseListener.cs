using KittyEngine.Core.Client.Outputs;
using System.Windows.Controls;

namespace KittyEngine.Core.Client.Input.WPFMouse
{
    public interface IWPFMouseListener : IInputHandler
    {
        void RegisterMouseEvents(IGameHost host);

        void Reset();

        bool IsEnabled { get; set; }
    }
}
