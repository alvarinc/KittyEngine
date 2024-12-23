using KittyEngine.Core.Client;
using KittyEngine.Core.Client.Model;
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Player Name:");

            var player = new PlayerInput(Guid.NewGuid().ToString(), Console.ReadLine());
            
            var server = new ServerInput("localhost", 9050);

            Console.WriteLine("Starting client...");

            ServiceContainer.Instance
                .ConfigureGameClient()
                .Get<Core.Client.Client>()
                .Run(server, player);
        }
    }
}
