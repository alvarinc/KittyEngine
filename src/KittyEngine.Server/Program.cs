
using KittyEngine.Core;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Server;
using KittyEngine.Core.Server.Behaviors;
using KittyEngine.Core.Services.IoC;
using KittyEngine.SampleMaps;

namespace KittyEngine.Server
{
    internal class Program
    {
        public class CustomCompositionBehavior : CompositionBehavior
        {
            public override void OnStartup(IServiceContainer container)
            {
                container.Register<StartupBehavior>(ServiceBehavior.Scoped);
            }

            public override void OnConfigureServices(IServiceContainer container)
            {
                var behavoirContainer = container.Get<IServerBehaviorContainer>();
                behavoirContainer.AddBehavior<StartupBehavior>();
            }
        }

        public class StartupBehavior : ServerBehavior
        {
            private Core.Server.Server _server;

            public StartupBehavior(Core.Server.Server server)
            {
                _server = server;
            }

            public override void OnStartGame()
            {
                _server.SendMessage(new GameCommandInput("loadmap")
                    .AddArgument("name", "<Default Test Map>"));
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            Engine.RunServer(onloadBehaviors: behaviors =>
            {
                behaviors.Add(new RegisterSampleMapsBehavior(EngineRuntime.Server));
                behaviors.Add(new CustomCompositionBehavior());
            });
        }
    }
}
