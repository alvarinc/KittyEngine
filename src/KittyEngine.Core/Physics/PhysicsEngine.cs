
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
            var updated = false;
            double deltaTime = deltaTimeInMilliseconds / 100.0; // Convert milliseconds to seconds
            var zeroVector = new Vector3D(0, 0, 0);

            // Update physics here
            foreach (var player in gameState.Players.Values)
            {
                if (player.IsGrounded) 
                {
                    if (player.VerticalVelocity == 0 && player.Velocity == zeroVector)
                    {
                        continue;
                    }

                    if (player.VerticalVelocity == 0 && player.Velocity != zeroVector)
                    {
                        var groundCollisionParams = new CollisionDetectionParameters
                        {
                            RigidBody = player,
                            MoveDirection = new Vector3D(0, -1, 0),
                            MapBvhTree = gameState.MapBvhTree
                        };

                        var groundCollisionResult = _collisionManager.DetectCollisions(groundCollisionParams);

                        if (groundCollisionResult.HasCollision)
                        {
                            var points = _collisionManager.GetCollidedPoints(groundCollisionParams, groundCollisionResult);
                            var highestY = points.Count > 0 ? points.Max(x => x.Y) : groundCollisionParams.RigidBody.Position.Y;

                            player.Position = new Point3D(player.Position.X, highestY + .1, player.Position.Z);
                            player.Velocity = zeroVector;
                        }

                        player.IsGrounded = groundCollisionResult.HasCollision;
                        updated = true;
                    }

                    if (player.VerticalVelocity != 0)
                    {
                        player.IsGrounded = false;
                        updated = true;
                    }
                }

                if (!player.IsGrounded)
                {
                    // Apply gravity
                    player.VerticalVelocity += Gravity * deltaTime;

                    if (player.Velocity != zeroVector)
                    {
                        player.Velocity *= .99;

                        if (player.Velocity.Length < .1)
                        {
                            player.Velocity = zeroVector;
                        }
                    }

                    // Clamp to terminal velocity
                    if (player.VerticalVelocity < MaxFallSpeed)
                    {
                        player.VerticalVelocity = MaxFallSpeed;
                    }

                    // Update vertical position
                    var verticalMove = new Vector3D(0, player.VerticalVelocity * deltaTime, 0);

                    // Perform collision detection for vertical movement
                    var collisionParams = new CollisionDetectionParameters
                    {
                        RigidBody = player,
                        MoveDirection = verticalMove + player.Velocity,
                        MapBvhTree = gameState.MapBvhTree
                    };
                    var collisionResult = _collisionManager.DetectCollisions(collisionParams);

                    if (collisionResult.HasCollision)
                    {
                        // Resolve collision: Set grounded state and stop vertical velocity
                        player.IsGrounded = true;
                        player.VerticalVelocity = 0;
                        player.Velocity = zeroVector;

                        var points = _collisionManager.GetCollidedPoints(collisionParams, collisionResult);
                        var highestY = points.Count > 0 ? points.Max(x => x.Y) : collisionParams.RigidBody.Position.Y;

                        // Adjust position to rest on the ground
                        player.Position = new Point3D(player.Position.X, highestY + .1, player.Position.Z);
                    }
                    else
                    {
                        // Update player's position
                        player.Position += verticalMove + player.Velocity;
                        player.IsGrounded = false;
                    }

                    updated = true;
                }
            }

            return updated;
        }
    }
}
