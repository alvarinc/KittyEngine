namespace KittyEngine.Core.Services.IoC
{
    internal class LightFactory<TInterface> : ILightFactory<TInterface>
    {
        private Dictionary<string, Func<TInterface>> _commands = new Dictionary<string, Func<TInterface>>();
        protected IServiceContainer _container;

        public LightFactory(IServiceContainer container) 
        {
            _container = container;
        }

        public TInterface Get(string name)
        {
            if (_commands.ContainsKey(name))
            {
                return _commands[name].Invoke();
            }

            return default;
        }

        public ILightFactory<TInterface> Add<TImplementation>(string name)
            where TImplementation : class, TInterface
        {
            _commands[name] = () => _container.Get<TImplementation>();
            return this;
        }
    }
}
