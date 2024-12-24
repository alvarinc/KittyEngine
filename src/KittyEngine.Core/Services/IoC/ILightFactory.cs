namespace KittyEngine.Core.Services.IoC
{
    internal interface ILightFactory<TInterface>
    {
        string[] Keys { get; }

        ILightFactory<TInterface> Add<TImplementation>(string name)
            where TImplementation : class, TInterface;

        TInterface Get(string name);
    }
}
