using KittyEngine.Core.Client;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Client.Model;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Assets.Maps;
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
        /// <param name="configure">Custom configuration for server</param>
        public static void RunWPFClient(PlayerInput player, ServerInput server = null, Grid placeholder = null, Action<IServiceContainer> configure = null)
        {
            StartWPFClient(player, server, placeholder, configure).Join();
        }

        /// <summary>
        /// Run Game Server thread
        /// </summary>
        /// <param name="port">Server connexion infos. If not set or null, get default values</param>
        /// <param name="configure">Custom configuration for server</param>
        public static void RunServer(int port = 0, Action<IServiceContainer> configure = null)
        {
            StartServer(port, configure).Join();
        }

        /// <summary>
        /// Start Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        /// <returns>Client thread</returns>
        public static Thread StartConsoleClient(PlayerInput player, ServerInput server)
        {
            var container = _containerBuilder().ConfigureGameClient(ClientType.Terminal);

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
        /// <param name="configure">Custom configuration for server</param>
        /// <returns>Client thread</returns>
        public static Thread StartWPFClient(PlayerInput player, ServerInput server = null, Grid placeholder = null, Action<IServiceContainer> configure = null)
        {
            var container = _containerBuilder().ConfigureGameClient(ClientType.WPF);

            if (configure != null)
            {
                configure(container);
            }

            var gameHost = new GameHost();

            container.Get<IWPFKeyboardListener>().RegisterKeyboardEvents(gameHost);
            container.Get<IWPFMouseListener>().RegisterMouseEvents(gameHost);
            container.Get<Graphics.IRenderer>().RegisterGraphicOutput(gameHost);

            placeholder.Children.Add(gameHost);

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
        /// Start Game Server thread
        /// </summary>
        /// <param name="port">Server connexion infos. If not set or null, get default values</param>
        /// <param name="configure">Custom configuration for server</param>
        /// <returns>Server thread</returns>
        public static Thread StartServer(int port = 0, Action<IServiceContainer> configure = null)
        {
            var container = _containerBuilder().ConfigureGameServer();
            
            if (configure != null)
            {
                configure(container);
            }

            var server = container.Get<Core.Server.Server>();

            var thread = new Thread(() => server.Run(port));
            thread.Name = "GameServer";
            thread.Start();

            return thread;
        }
    }
}
