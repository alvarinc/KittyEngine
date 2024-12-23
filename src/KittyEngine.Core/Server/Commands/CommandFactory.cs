﻿
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Server.Commands
{
    internal class CommandFactory : LightFactory<IGameCommand>
    {
        public CommandFactory() 
        {
            Register<JoinCommand>("join");
            Register<ExitCommand>("exit");
            Register<MoveCommand>("move");
        }
    }
}
