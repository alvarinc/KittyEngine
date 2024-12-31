using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.Services.Logging;

namespace KittyEngine.Core.Server.Commands
{
    internal class MoveCommand3D : MoveCommand
    {
        public MoveCommand3D(ILogger logger, ICollisionManager collisionManager) : base(logger, collisionManager)
        {
        }
    }
}
