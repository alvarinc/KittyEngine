using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public interface IRigidBody
    {
        Point3D Position { get; set; }
        Vector3D LookDirection { get; set; }
        Vector3D UpDirection { get; set; }

        double SizeX { get; set; }
        double SizeY { get; set; }
        double SizeZ { get; set; }

        Vector3D Velocity { get; set; }
        bool IsGrounded { get; set; }

        Rect3D GetBounds(Point3D position);
    }
}
