
using KittyEngine.Core.State;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public class PhysicsEngine : IPhysicsEngine
    {
        private Vector3D _gravity = new Vector3D(0, -9.80665, 0); // g = 9.80665 Gravity acceleration (m/s^2)

        public void UpdatePhysics(GameState gameState, double deltaTimeInMilliseconds)
        {
            // Update physics here
            foreach(var player in gameState.Players)
            {
                // Apply gravity
                //player.Velocity += _gravity * deltaTimeInMilliseconds / 1000;
                //player.Position += player.Velocity * deltaTimeInMilliseconds / 1000;
            }
        }
    }
}
