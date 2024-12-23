using KittyEngine.Core.Server;

namespace KittyEngine.Core.Client.Input
{
    public interface IInputHandler
    {
        List<GameCommandInput> HandleEvents();
    }
}
