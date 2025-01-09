using KittyEngine.Core.Client;
using KittyEngine.Core.Client.Input;
using KittyEngine.Core.Client.Input.WPFKeyboard;
using KittyEngine.Core.Client.Input.WPFMouse;
using KittyEngine.Core.Client.Model;
using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.Server.Behaviors.Compositions;
using KittyEngine.Core.Services.Configuration;
using KittyEngine.Core.Services.IoC;
using KittyEngine.Core.State;
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
        /// <param name="onloadBehaviors">Custom behavior configuration</param>
        public static void RunConsoleClient(PlayerInput player, ServerInput server = null, Action<List<CompositionBehavior>> onloadBehaviors = null)
        {
            StartConsoleClient(player, server, onloadBehaviors).Join();
        }

        /// <summary>
        /// Run Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        /// <param name="placeholder">WPF grid for host game</param>
        /// <param name="onloadBehaviors">Custom behavior configuration</param>
        public static void RunWPFClient(PlayerInput player, ServerInput server = null, Grid placeholder = null, Action<List<CompositionBehavior>> onloadBehaviors = null)
        {
            StartWPFClient(player, server, placeholder, onloadBehaviors).Join();
        }

        /// <summary>
        /// Run Game Server thread
        /// </summary>
        /// <param name="port">Server connexion infos. If not set or null, get default values</param>
        /// <param name="onloadBehaviors">Custom behavior configuration</param>
        public static void RunServer(int port = 0, Action<List<CompositionBehavior>> onloadBehaviors = null)
        {
            StartServer(port, onloadBehaviors).Join();
        }

        /// <summary>
        /// Start Game Client thread
        /// </summary>
        /// <param name="player">Player informations</param>
        /// <param name="server">Server connexion infos. If not set or null, get default values</param>
        /// <param name="onloadBehaviors">Custom behavior configuration</param>
        /// <returns>Client thread</returns>
        public static Thread StartConsoleClient(PlayerInput player, ServerInput server, Action<List<CompositionBehavior>> onloadBehaviors = null)
        {
            var container = _containerBuilder();
            var compositionBehaviors = new List<CompositionBehavior>()
            {
                new Client.Behaviors.Compositions.RegisterServicesBehavior(ClientType.Console),
                new Client.Behaviors.Compositions.RegisterAndConfigureFpsBehavior(),
            };

            // Update list of startup behaviors if needed
            if (onloadBehaviors != null)
            {
                onloadBehaviors(compositionBehaviors);
            }

            // Call Startup 
            foreach (var behavior in compositionBehaviors)
            {
                behavior.OnStartup(container);
            }

            // Call OnConfigure services
            foreach (var behavior in compositionBehaviors)
            {
                behavior.OnConfigureServices(container);
            }

            // Start client
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
        /// <param name="placeholder">WPF grid for host game</param>
        /// <param name="onloadBehaviors">Custom behavior configuration</param>
        /// <returns>Client thread</returns>
        public static Thread StartWPFClient(PlayerInput player, ServerInput server = null, Grid placeholder = null, Action<List<CompositionBehavior>> onloadBehaviors = null)
        {
            var container = _containerBuilder();
            var compositionBehaviors = new List<CompositionBehavior>()
            {
                new Client.Behaviors.Compositions.RegisterServicesBehavior(ClientType.Desktop),
                new Client.Behaviors.Compositions.RegisterAndConfigureFpsBehavior(),
            };

            // Update list of startup behaviors if needed
            if (onloadBehaviors != null)
            {
                onloadBehaviors(compositionBehaviors);
            }

            // Call Startup 
            foreach (var behavior in compositionBehaviors)
            {
                behavior.OnStartup(container);
            }

            // Call OnConfigure services
            foreach (var behavior in compositionBehaviors)
            {
                behavior.OnConfigureServices(container);
            }

            // Configure game host
            var renderer = container.Get<Graphics.IRenderer>();
            var inputHandler = container.Get<IInputHandler>();
            var clientState = container.Get<ClientState>();
            var gameHost = new GameHost(inputHandler, clientState);

            placeholder.Children.Clear();
            placeholder.Children.Add(gameHost);

            inputHandler.RegisterEvents(gameHost);

            renderer.RegisterGraphicOutput(gameHost);

            // Start client
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
        /// <param name="onloadBehaviors">Custom behavior configuration</param>
        /// <returns>Server thread</returns>
        public static Thread StartServer(int port = 0, Action<List<CompositionBehavior>> onloadBehaviors = null)
        {
            var container = _containerBuilder();
            var compositionBehaviors = new List<CompositionBehavior>()
                .AddComposer<RegisterServicesBehavior>()
                .AddFpsServerBehaviors();

            // Update list of startup behaviors if needed
            if (onloadBehaviors != null)
            {
                onloadBehaviors(compositionBehaviors);
            }

            // Call Startup 
            foreach (var behavior in compositionBehaviors)
            {
                behavior.OnStartup(container);
            }

            // Call OnConfigure services
            foreach (var behavior in compositionBehaviors)
            {
                behavior.OnConfigureServices(container);
            }

            // Start server
            var server = container.Get<Core.Server.Server>();

            var thread = new Thread(() => server.Run(port));
            thread.Name = "GameServer";
            thread.Start();

            return thread;
        }
    }
}
