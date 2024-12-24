using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public interface IMovableBody
    {
        Point3D Position { get; set; }
        Vector3D LookDirection { get; set; }
        Vector3D UpDirection { get; set; }
    }
}
