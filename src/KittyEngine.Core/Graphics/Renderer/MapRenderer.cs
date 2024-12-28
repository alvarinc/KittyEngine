using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.State;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal class MapRenderer : IMapRenderer
    {
        private IOutputFactory _outputFactory;
        private IMapBuilder _mapBuilder;
        private ILayeredModel3DFactory _modelFactory;

        private ClientState _clientState;
        private Viewport3D _viewport3D;
        private PlayerCameraState _playerCameraState;

        public MapRenderer(IOutputFactory outputFactory, IMapBuilder mapBuilder, ILayeredModel3DFactory modelFactory, ClientState clientState)
        {
            _outputFactory = outputFactory;
            _mapBuilder = mapBuilder;
            _modelFactory = modelFactory;
            _clientState = clientState;
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
        }

        public void LoadMap(MapDefinition mapDefinition)
        {
            // Create map
            _clientState.Graphics.Map.Clear();
            _clientState.Graphics.Map.AddRange(_mapBuilder.Create(mapDefinition));

            // Attach world
            _viewport3D.Children.Clear();

            foreach (var layeredModel in _clientState.Graphics.Map)
            {
                _viewport3D.Children.Add(layeredModel.GetModel());
            }

            // Attach camera
            _viewport3D.Camera = _playerCameraState.Camera;
        }

        public void UpdateCamera()
        {
            var body = _clientState.GameState.GetPlayer(_clientState.ConnectedUser.Guid);
            _playerCameraState.Camera.Position = body.Position;
            _playerCameraState.Camera.LookDirection = body.LookDirection;
        }

        public void UpdatePlayers()
        {
            foreach (var player in _clientState.GameState.Players.Values)
            {
                if (!_clientState.Graphics.Players.ContainsKey(player.PeerId))
                {
                    var playerModel = _modelFactory.Build(CreatePlayerModel(player));
                    _clientState.Graphics.Players.Add(player.PeerId, playerModel);
                    _viewport3D.Children.Add(playerModel.GetModel());
                }

                var layeredModel = _clientState.Graphics.Players[player.PeerId];
                var rect3D = player.GetBounds(player.Position);
                layeredModel.Translate(new Vector3D(rect3D.X, rect3D.Y, rect3D.Z));
            }
        }

        private VolumeDefinition CreatePlayerModel(PlayerState playerState)
        {
            return new VolumeDefinition
            {
                Color = Colors.Red,
                Position = new Point3D(0, 0, 0),
                Metadata = new VolumeMetadata
                {
                    UseBackMaterial = playerState.Guid != _clientState.ConnectedUser.Guid,
                    XSize = playerState.SizeX,
                    YSize = playerState.SizeY,
                    ZSize = playerState.SizeZ
                }
            };
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
