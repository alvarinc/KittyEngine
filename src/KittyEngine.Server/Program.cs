
using KittyEngine.Core;

namespace KittyEngine.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            Engine.RunServer();
        }
    }
}
