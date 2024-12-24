using KittyEngine.Core.Client;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Client.Model;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Server;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Services.IoC;
using System.ComponentModel;
using System.Windows.Controls;

namespace KittyEngine.Core
{
    /// <summary>
    /// Bootstrapper for starting game client and game server
    /// </summary>
    public static class Engine
    {
        private static Func<IServiceContainer> _containerBuilder = () => new DependencyInjectionServiceContainer();

        /// <summary>
        /// Use a custom build logic for Dependencies Injection containers
        /// </summary>
        /// <param name="containerBuilder">Custom build logic for Dependencies Injection containers</param>
        public static void SetContainerBuilder(Func<IServiceContainer> containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        /// <summary>
        /// Create a Dependencies Injection container
        /// </summary>
        /// <returns>Created container</returns>
        public static IServiceContainer CreateContainer()
        {
            return _containerBuilder();
        }

        /// <summary>
        /// Run Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        public static void RunConsoleClient(PlayerInput player, ServerInput server = null)
        {
            StartConsoleClient(player, server).Join();
        }

        /// <summary>
        /// Run Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        /// <param name="parent">WPF grid for host game</param>
        public static void RunWPFClient(PlayerInput player, ServerInput server = null, Grid parent = null)
        {
            StartWPFClient(player, server, parent).Join();
        }

        /// <summary>
        /// Run Game Server thread
        /// </summary>
        /// <param name="port">Server connexion infos. If not set or null, get default values</param>
        public static void RunServer(int port = 0)
        {
            StartServer(port).Join();
        }

        /// <summary>
        /// Start Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        /// <returns>Client thread</returns>
        public static Thread StartConsoleClient(PlayerInput player, ServerInput server)
        {
            var container = _containerBuilder().ConfigureGameClient(ClientType.Console);

            var configuration = container.Get<IConfigurationService>();
            var client = container.Get<Core.Client.Client>();

            if (server == null)
            {
                server = configuration.GetDefaultServer();
            }

            var thread = new Thread(() => client.Run(player, server));
            thread.Name = "GameClient";
            thread.Start();

            return thread;
        }

        /// <summary>
        /// Start Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        /// <param name="parent">WPF grid for host game</param>
        /// <returns>Client thread</returns>
        public static Thread StartWPFClient(PlayerInput player, ServerInput server = null, Grid parent = null)
        {
            var container = _containerBuilder().ConfigureGameClient(ClientType.WPF);

            var configuration = container.Get<IConfigurationService>();
            var client = container.Get<Core.Client.Client>();

            if (server == null)
            {
                server = configuration.GetDefaultServer();
            }

            var gameHost = new GameHost();
            parent.Children.Add(gameHost);

            container.Get<IWPFKeyboardListener>().RegisterKeyboardEvents(gameHost);
            //container.GetService<IMouseListener>().RegisterMouseEvents(gameHost);
            //container.GetService<IRenderLogic>().RegisterGraphicOutput(gameHost);

            var thread = new Thread(() => client.Run(player, server));
            thread.Name = "GameClient";
            thread.Start();

            return thread;
        }

        /// <summary>
        /// Start Game Server thread
        /// </summary>
        /// <param name="port">Server connexion infos. If not set or null, get default values</param>
        /// <returns>Server thread</returns>
        public static Thread StartServer(int port = 0)
        {
            var container = _containerBuilder().ConfigureGameServer();

            var configuration = container.Get<IConfigurationService>();
            var server = container.Get<Core.Server.Server>();

            if (port == 0)
            {
                port = configuration.GetDefaultServer().Port;
            }

            var thread = new Thread(() => server.Run(port));
            thread.Name = "GameServer";
            thread.Start();

            return thread;
        }
    }
}
