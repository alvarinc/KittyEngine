using Microsoft.Extensions.DependencyInjection;

namespace KittyEngine.Core.Services.IoC
{
    public class DependencyInjectionServiceContainer : IServiceContainer
    {
        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;

        public DependencyInjectionServiceContainer() 
        {
            _serviceCollection = new ServiceCollection();
        }

        public void Register<TInterface>(ServiceBehavior behavior, Func<TInterface> builder)
            where TInterface : class
        {
            switch (behavior)
            {
                case ServiceBehavior.Transient:
                    _serviceCollection.AddTransient<TInterface>(provider => builder());
                    break;
                case ServiceBehavior.Scoped:
                    _serviceCollection.AddScoped<TInterface>(provider => builder());
                    break;
                default:
                    throw new InvalidOperationException($"Not supported behavior {behavior}");
            }
        }

        public void Register<TImplementation>(ServiceBehavior behavior)
            where TImplementation : class
        {
            switch (behavior)
            {
                case ServiceBehavior.Transient:
                    _serviceCollection.AddTransient<TImplementation>();
                    break;
                case ServiceBehavior.Scoped:
                    _serviceCollection.AddScoped<TImplementation>();
                    break;
                default:
                    throw new InvalidOperationException($"Not supported behavior {behavior}");
            }
        }

        public void Register<TInterface, TImplementation>(ServiceBehavior behavior)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            switch(behavior)
            {
                case ServiceBehavior.Transient:
                    _serviceCollection.AddTransient<TInterface, TImplementation>();
                    break;
                case ServiceBehavior.Scoped:
                    _serviceCollection.AddScoped<TInterface, TImplementation>();
                    break;
                default:
                    throw new InvalidOperationException($"Not supported behavior {behavior}");
            }
        }

        public TInterface Get<TInterface>()
            where TInterface : class
        {
            EnsureServiceProvider();

            return _serviceProvider.GetService<TInterface>();
        }

        public TInterface Get<TInterface>(Type interfaceType)
            where TInterface : class
        {
            EnsureServiceProvider();

            return _serviceProvider.GetService(interfaceType) as TInterface;
        }

        private void EnsureServiceProvider()
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            }
        }
    }
}
