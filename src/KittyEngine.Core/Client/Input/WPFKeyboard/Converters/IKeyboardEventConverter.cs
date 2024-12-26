using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input.WPFKeyboard.Converters
{
    internal interface IKeyboardEventConverter
    {
        GameCommandInput Convert(GameState gameState, string playerId, KeyboardInput input);
    }
}
