
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public interface IPrimitiveMoveService
    {
        Vector3D GetMoveLongitudinal(IMovableBody body, double moveStep);
        Vector3D GetMoveLateral(IMovableBody body, double moveStep);
        Vector3D GetHorizontalRotation(IMovableBody body, double angleInDegree);
        Vector3D GetVerticalRotation(IMovableBody body, double angleInDegree);
    }
}
