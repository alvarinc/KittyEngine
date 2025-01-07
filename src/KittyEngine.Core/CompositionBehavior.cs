using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core
{
    public abstract class CompositionBehavior
    {
        public virtual void OnStartup(IServiceContainer container)
        {
        }

        public virtual void OnConfigureServices(IServiceContainer container)
        {

        }
    }
}
