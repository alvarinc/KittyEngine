
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public interface IPrimitiveMoveService
    {
        Vector3D GetMoveLongitudinal(IRigidBody body, double moveStep);
        Vector3D GetMoveLateral(IRigidBody body, double moveStep);
        Vector3D GetHorizontalRotation(IRigidBody body, double angleInDegree);
        Vector3D GetVerticalRotation(IRigidBody body, double angleInDegree);
    }
}
