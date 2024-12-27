using KittyEngine.Core.Client.Model;

namespace KittyEngine.Core.Services.Configuration
{
    public interface IConfigurationService
    {
        ServerInput GetDefaultServer();

        MouseSettings GetMouseSettings();
    }
}
