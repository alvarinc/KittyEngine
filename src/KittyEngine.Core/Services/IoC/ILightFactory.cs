namespace KittyEngine.Core.Services.IoC
{
    internal interface ILightFactory<TInterface>
    {
        ILightFactory<TInterface> Register<TImplementation>(string name)
            where TImplementation : TInterface, new();

        TInterface Create(string name);
    }
}
