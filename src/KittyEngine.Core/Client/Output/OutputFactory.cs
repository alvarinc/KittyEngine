using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace KittyEngine.Core.Client.Outputs
{
    public class OutputFactory : IOutputFactory
    {
        private Func<Viewport3D> _viewportBuilder = () => new Viewport3D
        { 
            ClipToBounds = true, 
            Focusable = true, 
            Cursor = Cursors.None,
        };

        public Viewport3D CreateViewport3D()
        {
            return _viewportBuilder();
        }

        public void RegisterViewport(Func<Viewport3D> builder)
        {
            _viewportBuilder = builder;
        }
    }
}
