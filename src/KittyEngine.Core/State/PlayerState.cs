using KittyEngine.Core.Physics;
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

        public PlayerState(int peerId)
        {
            PeerId = peerId;
            Position = new Point3D(0, 0, 0);
            LookDirection = new Vector3D(0, 0, -1);
            UpDirection = new Vector3D(0, 1, 0);
        }
    }
}
