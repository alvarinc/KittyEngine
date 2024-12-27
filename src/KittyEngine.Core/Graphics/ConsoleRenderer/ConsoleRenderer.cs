using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;

namespace KittyEngine.Core.Graphics.ConsoleRenderer
{
    internal class ConsoleRenderer : IRenderer
    {
        private IGameHost _host;

        public void RegisterGraphicOutput(IGameHost host)
        {
            _host = host;
        }

        public void Render(GameState _gameState, string playerId)
        {
            var player = _gameState.GetPlayer(playerId);
            if (player == null)
            {
                Console.WriteLine(">  Not on map yet.");
                return;
            }

            Console.WriteLine($">  connection {player.PeerId} : {player.Name}");
            //Console.WriteLine(">>>>");
            //Console.WriteLine(">>>>");
            //Console.WriteLine(">>>>");
            //Console.WriteLine(">>>>");
            //Console.WriteLine(">>>>");
            //Console.WriteLine(">>>>");

            //for (int column = _gameState.Map.MinZ; column <= _gameState.Map.MaxZ; column++)
            //{
            //    for (int row = _gameState.Map.MinX; row <= _gameState.Map.MaxX; row++)
            //    {
            //        if (_gameState.Players.Values.Any(p => -p.Position.Z == column && p.Position.X == row))
            //        {
            //            if (-player.Position.Z == column && player.Position.X == row)
            //            {
            //                Console.Write("X");
            //            }
            //            else
            //            {
            //                Console.Write("@");
            //            }
            //        }
            //        else if (column == _gameState.Map.MinZ || column == _gameState.Map.MaxZ || row == _gameState.Map.MinX || row == _gameState.Map.MaxX)
            //        {
            //            Console.Write("*");
            //        }
            //        else
            //        {
            //            Console.Write("_");
            //        }

            //        if (row == _gameState.Map.MaxX)
            //        {
            //            Console.WriteLine();
            //        }
            //    }
            //}

            Console.WriteLine(">>>>");
        }
    }
}
