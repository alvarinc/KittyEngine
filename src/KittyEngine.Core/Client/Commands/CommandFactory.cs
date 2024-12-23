
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client.Commands
{
    internal class CommandFactory : LightFactory<IGameCommand>
    {
        public CommandFactory() 
        {
            Register<SynchronizeCommand>("sync");
        }
    }
}
