namespace KittyEngine.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Player Name:");
            var playerName = Console.ReadLine();
            var playerId = Guid.NewGuid().ToString();

            Console.WriteLine("Starting client...");
            var client = new KittyEngine.Core.Client.Client();
            client.Run("localhost", 9050, playerId, playerName);
        }
    }
}
