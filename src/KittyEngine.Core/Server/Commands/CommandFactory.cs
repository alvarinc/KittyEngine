
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Server.Commands
{
    internal class CommandFactory : LightFactory<IGameCommand>
    {
        public CommandFactory(IServiceContainer _container) : base(_container)
        {
            Add<JoinCommand>("join");
            Add<ExitCommand>("exit");
            Add<MoveCommand>("move");
            Add<MoveCommand3D>("move3d");
            Add<RotateCommand3D>("rotate3d");
        }
    }
}
