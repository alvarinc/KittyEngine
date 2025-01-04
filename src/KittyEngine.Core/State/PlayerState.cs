using KittyEngine.Core.Physics;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.State
{
    public class PlayerState : MovableBody
    {
        public int PeerId { get; }
        public string Guid { get; set; }
        public string Name { get; set; }

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
    }
}
