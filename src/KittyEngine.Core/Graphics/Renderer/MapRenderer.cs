﻿using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Physics;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Renderer
{
    internal class MapRenderer : IMapRenderer
    {
        private IOutputFactory _outputFactory;
        private IMapBuilder _mapBuilder;
        private Viewport3D _viewport3D;
        private PlayerCameraState _playerCameraState;

        public MapRenderer(IOutputFactory outputFactory, IMapBuilder mapBuilder)
        {
            _outputFactory = outputFactory;
            _mapBuilder = mapBuilder;
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
            var layeredModels = _mapBuilder.Create(mapDefinition);

            // Attach world
            _viewport3D.Children.Clear();

            foreach (var layeredModel in layeredModels)
            {
                _viewport3D.Children.Add(layeredModel.GetModel());
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