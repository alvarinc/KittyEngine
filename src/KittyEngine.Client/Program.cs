using KittyEngine.Core;
using KittyEngine.Core.Client.Model;

namespace KittyEngine.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var guid = Guid.NewGuid().ToString();
            var player = new PlayerInput(guid, $"Player-{guid}");

            Console.WriteLine("Starting client...");

            Engine.RunConsoleClient(player);
        }
    }
}
