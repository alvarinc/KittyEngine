
namespace KittyEngine.Core.Services.IoC
{
    public static class ServiceContainer
    {
        private static IServiceContainer _serviceContainer;

        public static IServiceContainer Instance => _serviceContainer;

        static ServiceContainer()
        {
            Set(new DependencyInjectionServiceContainer());
        }

        public static IServiceContainer Set(IServiceContainer container)
        {
            _serviceContainer = container;
            return _serviceContainer;
        }
    }
}
