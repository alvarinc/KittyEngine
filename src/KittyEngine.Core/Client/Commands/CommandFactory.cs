
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client.Commands
{
    internal class CommandFactory : LightFactory<IGameCommand>
    {
        public CommandFactory(IServiceContainer _container) : base(_container)
        {
            Add<JoinedCommand>("joined");
            Add<SynchronizeCommand>("sync");
        }
    }
}
