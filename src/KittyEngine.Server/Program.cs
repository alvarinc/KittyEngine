
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            ServiceContainer.Instance
                .ConfigureGameServer()
                .Get<Core.Server.Server>()
                .Run(9050);
        }
    }
}
