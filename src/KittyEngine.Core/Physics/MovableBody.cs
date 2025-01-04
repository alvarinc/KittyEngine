using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public class MovableBody : IMovableBody
    {
        public Point3D Position { get; set; }
        public Vector3D LookDirection { get; set; }
        public Vector3D UpDirection { get; set; }

        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }

        public double VerticalVelocity { get; set; }
        public bool IsGrounded { get; set; }

        public MovableBody() { }

        public MovableBody(IMovableBody body)
        {
            Position = body.Position;
            LookDirection = body.LookDirection;
            UpDirection = body.UpDirection;

            SizeX = body.SizeX;
            SizeY = body.SizeY;
            SizeZ = body.SizeZ;
        }

        public Rect3D GetBounds(Point3D position)
        {
            var originX = position.X - SizeX / 2d;
            var originY = position.Y;
            var originZ = position.Z - SizeZ / 2d;

            return new Rect3D(originX, originY, originZ, SizeX, SizeY, SizeZ);
        }
    }
}
