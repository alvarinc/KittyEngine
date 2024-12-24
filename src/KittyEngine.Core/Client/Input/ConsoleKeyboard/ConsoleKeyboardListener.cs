using KittyEngine.Core.Client.Input.ConsoleKeyboard.Converters;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard
{
    public class ConsoleKeyboardListener : IInputHandler
    {
        private LightFactory<IKeyboardEventConverter> _commandFactory;

        public ConsoleKeyboardListener(IServiceContainer _container)
        {
            _commandFactory = new LightFactory<IKeyboardEventConverter>(_container);
            _commandFactory.Add<ConsoleKeyboard.Converters.ExitConverter>("exit");
            _commandFactory.Add<ConsoleKeyboard.Converters.MoveConverter>("move");
        }

        public List<GameCommandInput> HandleEvents(GameState gameState, string playerId)
        {
            var results = new List<GameCommandInput>();
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                string keyPressed = keyInfo.Key.ToString();

                foreach (var key in _commandFactory.Keys)
                {
                    var cmd = _commandFactory
                        .Get(key)
                        .Convert(gameState, playerId, keyPressed);

                    if (cmd != null)
                    {
                        results.Add(cmd);
                        break;
                    }
                }
            }

            return results;
        }
    }
}
