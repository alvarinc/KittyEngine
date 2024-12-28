using KittyEngine.Core.Physics;
using KittyEngine.Core.Services.Configuration;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.State
{
    public class PlayerState : IMovableBody
    {
        public int PeerId { get; }
        public string Guid { get; set; }
        public string Name { get; set; }

        #region IMovableBody specific
        public Point3D Position { get; set; }
        public Vector3D LookDirection { get; set; }
        public Vector3D UpDirection { get; set; }
        #endregion

        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }
        
        public PlayerState(int peerId)
        {
            PeerId = peerId;
            Position = new Point3D(0, 0, 0);
            LookDirection = new Vector3D(0, 0, -1);
            UpDirection = new Vector3D(0, 1, 0);

            SizeX = 6;
            SizeY = 12;
            SizeZ = 6;
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
