
using KittyEngine.Core;
using KittyEngine.Core.Server.Behaviors.Compositions;
using KittyEngine.SampleMaps;

namespace KittyEngine.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            Engine.RunServer(onloadBehaviors: behaviors =>
            {
                behaviors
                    .AddComposer(new RegisterSampleAssetsBehavior())
                    .AddComposer(new RegisterSampleMapsBehavior())
                    .AddServerBehavior<StartupBehavior>();
            });
        }
    }
}
