using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Graphics.Assets.Maps.Predefined;
using KittyEngine.Core.Physics;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.WPFRenderer
{
    internal class WorldLoader
    {
        private IOutputFactory _outputFactory;
        private Viewport3D _viewport3D;
        private PlayerCameraState _playerCameraState;

        public WorldLoader(IOutputFactory outputFactory)
        {
            _outputFactory = outputFactory;
        }

        public void BindGraphicsToViewport(IGameHost host)
        {
            // Ensure viewport is created
            if (host.Viewport3D == null)
            {
                _viewport3D = _outputFactory.CreateViewport3D();
                host.AttachViewport(_viewport3D);

                _playerCameraState = InitializeCamera();
            }

            var map = new MapRenderer(new NewMapBuilder().CreateMap());
            var level = map.Render();
            // Attach world
            _viewport3D.Children.Clear();

            foreach (var model in level)
            {
                _viewport3D.Children.Add(model.GetModel());
            }

            // Attach camera
            _viewport3D.Camera = _playerCameraState.Camera;
        }

        public void UpdateCamera(IMovableBody body)
        {
            _playerCameraState.Camera.Position = body.Position;
            _playerCameraState.Camera.LookDirection = body.LookDirection;
        }

        private PlayerCameraState InitializeCamera()
        {
            var cameraContext = new PlayerCameraState();
            cameraContext.Camera = new PerspectiveCamera
            {
                UpDirection = new Vector3D(0, 1, 0),
                LookDirection = new Vector3D(0, 0, -1),
                Position = new Point3D(10, 10, 10),
                FieldOfView = 90,
                FarPlaneDistance = 20000,
                NearPlaneDistance = .1
            };

            cameraContext.RotationYFromOrigin = CreateRotation(new Vector3D(0, 1, 0), 0);
            cameraContext.RotationXFromOrigin = CreateRotation(new Vector3D(1, 0, 0), 0);
            cameraContext.RotationZFromOrigin = CreateRotation(new Vector3D(0, 0, 1), 0);

            return cameraContext;
        }

        private AxisAngleRotation3D CreateRotation(Vector3D axis, double angle)
        {
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = axis;
            myAxisAngleRotation3d.Angle = angle;
            return myAxisAngleRotation3d;
        }
    }
}
