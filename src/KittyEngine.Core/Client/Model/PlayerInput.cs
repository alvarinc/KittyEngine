namespace KittyEngine.Core.Client.Model
{
    public class PlayerInput
    {
        public string Guid { get; }
        public string Name { get; }

        public PlayerInput(string guid, string name)
        {
            Guid = guid;
            Name = name;
        }
    }
}
