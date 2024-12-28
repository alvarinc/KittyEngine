
using KittyEngine.Core.Graphics.Models.Definitions;

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

        public PlayerState GetPlayer(string guid) 
        {
            return Players.Values.FirstOrDefault(x => x.Guid == guid);
        }
    }
}
