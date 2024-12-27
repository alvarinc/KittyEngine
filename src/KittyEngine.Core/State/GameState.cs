
using KittyEngine.Core.Graphics.Assets.Maps.Predefined;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Physics;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.State
{
    public enum GameStatus
    {
        Creating,
        Created,
        Running,
        Terminated
    }

    public class GameState
    {
        public GameState()
        {
            Id = Guid.NewGuid().ToString();
            Map = new MapDefinition();
            Status = GameStatus.Creating;
        }

        public string Id { get; set; }

        public GameStatus Status { get; set; }

        public Dictionary<int, PlayerState> Players { get; set; } = new();

        public MapDefinition Map { get; set; }

        public PlayerState GetPlayer(string guid) 
        {
            return Players.Values.FirstOrDefault(x => x.Guid == guid);
        }
    }

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
            Position = new Point3D(0,0,0);
            LookDirection = new Vector3D(0, 0, -1);
            UpDirection = new Vector3D(0, 1, 0);
        }
    }

    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
