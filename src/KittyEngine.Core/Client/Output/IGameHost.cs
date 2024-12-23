using System.Windows.Controls;
using System.Windows.Threading;

namespace KittyEngine.Core.Client.Outputs
{
    public interface IGameHost : ISynchronizableOutput
    {
        Dispatcher Dispatcher { get; }

        void AttachViewport(Viewport3D viewport);
    }
}
