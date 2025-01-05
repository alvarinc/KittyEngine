
using KittyEngine.Core.Physics.Collisions;
using KittyEngine.Core.State;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Physics
{
    public class PhysicsEngine : IPhysicsEngine
    {
        private const double Gravity = -9.80665; // Gravity acceleration (m/s^2)
        private const double MaxFallSpeed = -50.0; // Terminal velocity

        private ICollisionManager _collisionManager;

        public PhysicsEngine(ICollisionManager collisionManager)
        {
            _collisionManager = collisionManager;
        }

        public bool UpdatePhysics(GameState gameState, double deltaTimeInMilliseconds)
        {
            return false; // Bypass before implementation
            var updated = false;
            double deltaTime = deltaTimeInMilliseconds / 1000.0; // Convert milliseconds to seconds

            // Update physics here
            foreach (var player in gameState.Players.Values)
            {
                if (player.IsGrounded && player.VerticalVelocity == 0)
                {
                    continue;
                }

                if (!player.IsGrounded)
                {
                    // Apply gravity
                    player.VerticalVelocity += Gravity * deltaTime;

                    // Clamp to terminal velocity
                    if (player.VerticalVelocity < MaxFallSpeed)
                    {
                        player.VerticalVelocity = MaxFallSpeed;
                    }
                }

                // Update vertical position
                var verticalMove = new Vector3D(0, player.VerticalVelocity * deltaTime, 0);

                // Perform collision detection for vertical movement
                var collisionParams = new CollisionDetectionParameters
                {
                    RigidBody = player,
                    MoveDirection = verticalMove,
                    MapBvhTree = gameState.MapBvhTree
                };
                var collisionResult = _collisionManager.DetectCollisions(collisionParams);

                if (collisionResult.HasCollision)
                {
                    // Resolve collision: Set grounded state and stop vertical velocity
                    player.IsGrounded = true;
                    player.VerticalVelocity = 0;

                    // Adjust position to rest on the ground
                    var highestCollisionPoint = collisionResult.Collisions
                        .SelectMany(c => c.CollidedTriangles)
                        .Max(t => t.GetHighestPoint().Y);
                    player.Position = new Point3D(player.Position.X, highestCollisionPoint + .1, player.Position.Z);
                }
                else
                {
                    // Update player's position
                    player.Position += verticalMove;
                    player.IsGrounded = false;
                }

                updated = true;
            }

            return updated;
        }
    }
}
