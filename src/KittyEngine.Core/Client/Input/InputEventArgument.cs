using KittyEngine.Core.Server;

namespace KittyEngine.Core.Client.Input
{
    internal class InputEventArgument
    {
        public List<GameCommandInput> Inputs { get; set; } = new();
    }
}
