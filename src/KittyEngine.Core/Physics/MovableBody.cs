using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    internal class MovableBody : IMovableBody
    {
        public Point3D Position { get; set; }
        public Vector3D LookDirection { get; set; }
        public Vector3D UpDirection { get; set; }

        public MovableBody() { }

        public MovableBody(IMovableBody body)
        {
            Position = body.Position;
            LookDirection = body.LookDirection;
            UpDirection = body.UpDirection;
        }
    }
}
