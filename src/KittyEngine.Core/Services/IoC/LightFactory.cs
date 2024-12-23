namespace KittyEngine.Core.Services.IoC
{
    internal class LightFactory<TInterface> : ILightFactory<TInterface>
    {
        private Dictionary<string, Func<TInterface>> _commands = new Dictionary<string, Func<TInterface>>();

        public TInterface Create(string name)
        {
            if (_commands.ContainsKey(name))
            {
                return _commands[name].Invoke();
            }

            return default;
        }

        public ILightFactory<TInterface> Register<TImplementation>(string name)
            where TImplementation : TInterface, new()
        {
            _commands[name] = () => new TImplementation();
            return this;
        }
    }
}
