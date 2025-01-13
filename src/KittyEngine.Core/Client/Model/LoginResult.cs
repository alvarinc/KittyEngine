using KittyEngine.Core.Services.Configuration;

namespace KittyEngine.Core.Client.Model
{
    public class LoginResult
    {
        public static LoginResult GetDefault()
        {
            var guid = Guid.NewGuid();

            return new LoginResult()
            {
                PlayerInput = new PlayerInput(guid.ToString(), $"Player-{guid}"),
                ServerInput = new ConfigurationService().GetDefaultServer()
            };
        }

        public ServerInput ServerInput { get; set; }

        public PlayerInput PlayerInput { get; set; }
    }
}
