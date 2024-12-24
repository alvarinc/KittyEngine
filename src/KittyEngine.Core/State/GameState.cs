
using KittyEngine.Core.Physics;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.State
{
    public enum GameStatus
    {
        Creating,
        Created,
        Play,
        Terminated
    }

    public class GameState
    {
        public GameState()
        {
            Map = MapDescription.DefaultMap;
            Status = GameStatus.Creating;
        }

        public GameStatus Status { get; set; }

        public Dictionary<int, PlayerState> Players { get; set; } = new();

        public MapDescription Map { get; set; } = new();

        public PlayerState GetPlayer(string guid) 
        {
            return Players.Values.FirstOrDefault(x => x.Guid == guid);
        }
    }

    public class MapDescription
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int MaxZ { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MinZ { get; set; }

        public static MapDescription DefaultMap => new MapDescription
        {
            MaxX = 10,
            MaxY = 10,
            MaxZ = 10,
            MinX = -10,
            MinY = -10,
            MinZ = -10
        };
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
