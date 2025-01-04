using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public class PrimitiveMoveService : IPrimitiveMoveService
    {
        public Vector3D GetMoveLongitudinal(IRigidBody body, double moveStep)
        {
            Vector3D direction = new Vector3D(body.LookDirection.X, 0, body.LookDirection.Z);
            direction.Normalize();

            return direction * moveStep;
        }

        public Vector3D GetMoveLateral(IRigidBody body, double moveStep)
        {
            var direction = Vector3D.CrossProduct(body.UpDirection, body.LookDirection);
            direction.Normalize();

            return direction * moveStep;
        }

        public Vector3D GetHorizontalRotation(IRigidBody body, double angleInDegree)
        {
            var m = new Matrix3D();
            
            // Rotate about the camera's up direction to look left/right
            m.Rotate(new Quaternion(body.UpDirection, angleInDegree)); 

            return m.Transform(body.LookDirection);
        }

        public Vector3D GetVerticalRotation(IRigidBody body, double angleInDegree)
        {
            // Cross Product gets a vector that is perpendicular to the passed in vectors (order does matter, reverse the order and the vector will point in the reverse direction)
            var cp = Vector3D.CrossProduct(body.UpDirection, body.LookDirection);
            cp.Normalize();

            var m = new Matrix3D();
            m.Rotate(new Quaternion(cp, angleInDegree)); // Rotate about the vector from the cross product
            var lookDirection = m.Transform(body.LookDirection);

            // Avoid glitch at render (scene disapear) : don't rotate if new LookDirection is too close to UpDirection
            var glitch1 =
                Math.Abs(Math.Round(lookDirection.X, 10)) == Math.Abs(Math.Round(body.UpDirection.X, 10)) &&
                Math.Abs(Math.Round(lookDirection.Y, 10)) == Math.Abs(Math.Round(body.UpDirection.Y, 10)) &&
                Math.Abs(Math.Round(lookDirection.Z, 10)) == Math.Abs(Math.Round(body.UpDirection.Z, 10));

            // Avoid glitch at render (scene disapear) : don't rotate if new LookDirection is too close to UpDirection
            var glitch2 =
                Math.Abs(Math.Round(lookDirection.X, 10)) == Math.Abs(Math.Round(body.UpDirection.X, 10)) &&
                Math.Abs(Math.Round(lookDirection.Z, 10)) == Math.Abs(Math.Round(body.UpDirection.Z, 10));

            // Avoid glitch at render : scene disapear or position invertion
            var glitch3 =
                double.IsNaN(lookDirection.X)
                || double.IsNaN(lookDirection.Z)
                || Math.Sign(lookDirection.X) != Math.Sign(body.LookDirection.X)
                || Math.Sign(lookDirection.Z) != Math.Sign(body.LookDirection.Z);

            if (glitch1 || glitch2 || glitch3)
            {
                return body.LookDirection;
            }

            return lookDirection;
        }
    }
}
