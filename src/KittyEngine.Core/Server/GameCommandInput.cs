namespace KittyEngine.Core.Server
{
    public class GameCommandInput
    {
        public string Command { get; set; }
        public Dictionary<string, string> Args { get; set; }

        public GameCommandInput()
        {

        }

        public GameCommandInput(string commandName)
        {
            Command = commandName;
            Args = new Dictionary<string, string>();
        }

        public GameCommandInput AddArgument(string key, string value)
        {
            Args.Add(key, value);
            return this;
        }
    }
}
