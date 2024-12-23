
namespace KittyEngine.Core.Services.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private List<ILogConnector> _connectors = new List<ILogConnector>();
        private bool _isEnabled = true;

        public bool IsEnabled => _isEnabled;
        public IEnumerable<ILogConnector> Connectors => _connectors;

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void AddConnector(ILogConnector listener)
        {
            _connectors.Add(listener);
        }
    }
}
