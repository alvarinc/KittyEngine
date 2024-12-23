using KittyEngine.Core.Client;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.State;

namespace KittyEngine.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Player Name:");

            var player = new Player(Guid.NewGuid().ToString(), Console.ReadLine());
            
            var gameServer = new GameServer("localhost", 9050);

            Console.WriteLine("Starting client...");

            ServiceContainer.Instance
                .ConfigureGameClient()
                .Get<Core.Client.Client>()
                .Run(gameServer, player);
        }
    }
}
