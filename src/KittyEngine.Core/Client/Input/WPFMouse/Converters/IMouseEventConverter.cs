using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input.WPFMouse.Converters
{
    internal interface IMouseEventConverter
    {
        GameCommandInput Convert(GameState gameState, string playerId, MouseInput input);
    }
}
