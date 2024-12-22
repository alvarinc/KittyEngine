namespace KittyEngine.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var server = new KittyEngine.Core.Server.Server();
            server.Run(9050);
        }
    }
}
