
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Graphics.Models.Definitions;
using KittyEngine.Core.Physics.Collisions.BVH;
using Newtonsoft.Json;

namespace KittyEngine.Core.State
{
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

        [JsonIgnore]
        public BVHTreeNode<LayeredModel3D> MapBvhTree { get; set; }

        public PlayerState GetPlayer(string guid) 
        {
            return Players.Values.FirstOrDefault(x => x.Guid == guid);
        }
    }
}
