using System;
using System.Windows.Controls;

namespace KittyEngine.Core.Client.Outputs
{
    public interface IOutputFactory
    {
        Viewport3D CreateViewport3D();

        void RegisterViewport(Func<Viewport3D> builder);

        UserControl CreateOutput(OutputTypes type);
    }
}
