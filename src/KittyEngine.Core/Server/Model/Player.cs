namespace KittyEngine.Core.Server.Model
{
    public class Player
    {
        public int PeerId { get; }
        public string Guid { get; set; }
        public string Name { get; set; }

        public Player(int peerId) 
        {
            PeerId = peerId;
        }
    }
}
