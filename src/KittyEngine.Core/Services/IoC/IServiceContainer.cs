
namespace KittyEngine.Core.Services.IoC
{
    public interface IServiceContainer
    {
        void Register<TInterface>(ServiceBehavior behavior, Func<TInterface> builder)
            where TInterface : class;

        void Register<TInterface, TImplementation>(ServiceBehavior behavior)
            where TInterface : class
            where TImplementation : class, TInterface;

        void Register<TImplementation>(ServiceBehavior behavior)
            where TImplementation : class;

        TInterface Get<TInterface>()
            where TInterface : class;

        TInterface Get<TInterface>(Type interfaceType)
            where TInterface : class;
    }
}
