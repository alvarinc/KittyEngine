using KittyEngine.Core.Server.Behaviors;
using KittyEngine.Core.Server;

namespace KittyEngine.Server
{
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
}
