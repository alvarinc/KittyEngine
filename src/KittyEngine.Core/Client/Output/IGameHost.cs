using System.Windows.Controls;
using System.Windows.Threading;

namespace KittyEngine.Core.Client.Outputs
{
    public interface IGameHost : ISynchronizableOutput
    {
        Dispatcher Dispatcher { get; }

        Viewport3D Viewport3D { get; }

        UserControl HostControl { get; }

        void AttachViewport(Viewport3D viewport);
    }
}
