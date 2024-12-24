using KittyEngine.Core.Server;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard.Converters
{
    internal interface IKeyboardEventConverter
    {
        GameCommandInput Convert(GameState gameState, string playerId, string keyPressed);
    }
}
