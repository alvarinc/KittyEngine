
namespace KittyEngine.Core.Services.Logging
{
    public interface ILoggerManager
    {
        bool IsEnabled { get; }
        IEnumerable<ILogConnector> Connectors { get; }

        void Enable();
        void Disable();
        void AddConnector(ILogConnector listener);
    }
}
