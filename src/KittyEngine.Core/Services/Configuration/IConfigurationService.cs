using KittyEngine.Core.Client.Model;

namespace KittyEngine.Core.Services.Configuration
{
    internal interface IConfigurationService
    {
        ServerInput GetDefaultServer();
    }
}
