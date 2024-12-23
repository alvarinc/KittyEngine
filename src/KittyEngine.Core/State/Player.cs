
namespace KittyEngine.Core.State
{
    public class Player
    {
        public string Guid { get; set; }
        public string Name { get; set; }

        public Player() { }

        public Player(string guid, string name)
        {
            Guid = guid;
            Name = name;
        }
    }
}
