
using KittyEngine.Core;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Server;
using KittyEngine.SampleMaps;
using KittyEngine.SampleMaps.Maze;

namespace KittyEngine.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            Engine.RunServer(configure:container => 
            {
                var contentService = container.Get<IContentService>();
                contentService.RegisterContentFromSampleMaps();

                var mapBuilderFactory = container.Get<IMapBuilderFactory>();
                mapBuilderFactory.RegisterMapsFromAssets();
                mapBuilderFactory.RegisterMap(new MazeMapBuilder());

                var server = container.Get<Core.Server.Server>();
                server.SendMessage(new GameCommandInput("loadmap")
                    .AddArgument("name", "<Default Test Map>"));
            });
        }
    }
}
