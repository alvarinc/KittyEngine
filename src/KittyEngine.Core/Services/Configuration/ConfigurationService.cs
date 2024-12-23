using KittyEngine.Core.Client.Model;

namespace KittyEngine.Core.Services.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        public ServerInput GetDefaultServer()
        {
            return new ServerInput("localhost", 9050);
        }
    }
}
