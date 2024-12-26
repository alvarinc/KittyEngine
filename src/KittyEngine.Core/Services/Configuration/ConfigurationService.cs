using KittyEngine.Core.Client.Model;

namespace KittyEngine.Core.Services.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        public ServerInput GetDefaultServer()
        {
            return new ServerInput("localhost", 9050);
        }

        public MouseSettings GetMouseSettings()
        {
            return new MouseSettings
            {
                MoveSpeed = .75,
                MouseRotationSpeed = .5,
                MouseXInverted = false,
                MouseYInverted = false
            };
        }
    }
}
