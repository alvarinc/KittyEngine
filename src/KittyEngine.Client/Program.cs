using KittyEngine.Core;
using KittyEngine.Core.Client.Model;

namespace KittyEngine.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Player Name:");

            var player = new PlayerInput(Guid.NewGuid().ToString(), Console.ReadLine());

            Console.WriteLine("Starting client...");

            Engine.RunClient(player);
        }
    }
}
