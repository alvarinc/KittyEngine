using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.State;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal class GameWorldRenderer : IGameWorldRenderer
    {
        private IGameWorldBuilder _gameWorldBuilder;
        private ILayeredModel3DFactory _modelFactory;

        private ClientState _clientState;
        private Viewport3D _viewport3D;
        private PlayerCameraState _playerCameraState;

        public GameWorldRenderer(IGameWorldBuilder gameWorldBuilder, ILayeredModel3DFactory modelFactory, ClientState clientState)
        {
            _gameWorldBuilder = gameWorldBuilder;
            _modelFactory = modelFactory;
            _clientState = clientState;
        }

        public void RegisterOutput(IGameHost host)
        {
            _viewport3D = host.Viewport3D;
        }

        public void LoadGameWorld(MapDefinition mapDefinition)
        {
            // Create map
            _clientState.Graphics.Map.Clear();
            _clientState.Graphics.Map.AddRange(_gameWorldBuilder.Create(mapDefinition));

            // Attach world
            _viewport3D.Children.Clear();

            foreach (var layeredModel in _clientState.Graphics.Map)
            {
                _viewport3D.Children.Add(layeredModel.GetModel());
            }

            // Attach camera
            _playerCameraState = CreateCamera();
            _viewport3D.Camera = _playerCameraState.Camera;
        }

        public void UpdateCamera()
        {
            var body = _clientState.GameState.GetPlayer(_clientState.ConnectedUser.Guid);
            _playerCameraState.Camera.Position = body.Position + new Vector3D(0, 10, 0);
            _playerCameraState.Camera.LookDirection = body.LookDirection;
        }

        public void UpdatePlayers()
        {
            foreach (var player in _clientState.GameState.Players.Values)
            {
                if (player.PeerId == _clientState.ConnectedUser.PeerId)
                {
                    // Skip self
                    continue;
                }

                if (!_clientState.Graphics.Players.ContainsKey(player.PeerId))
                {
                    var playerModel = CreatePlayerModel(player);
                    _clientState.Graphics.Players.Add(player.PeerId, playerModel);
                    _viewport3D.Children.Add(playerModel.GetModel());
                }

                var layeredModel = _clientState.Graphics.Players[player.PeerId];
                var rect3D = player.GetBounds(player.Position);
                layeredModel.Translate(new Vector3D(rect3D.X, rect3D.Y, rect3D.Z));
            }
        }

        private LayeredModel3D CreatePlayerModel(PlayerState playerState, double opacity = .5)
        {
            var definition = new VolumeDefinition
            {
                Color = Colors.Red,
                Position = new Point3D(0, 0, 0),
                Metadata = new VolumeMetadata
                {
                    UseBackMaterial = true,
                    XSize = playerState.SizeX,
                    YSize = playerState.SizeY,
                    ZSize = playerState.SizeZ,
                    Opacity = opacity
                }
            };

            return _modelFactory.Build(definition);
        }

        private PlayerCameraState CreateCamera()
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
