using KittyEngine.Core.Physics.Collisions.SAT;
using KittyEngine.Core.Graphics;
using System.Windows.Media.Media3D;
using KittyEngine.Core.Graphics.Models.Builders;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionManager : ICollisionManager
    {
        public CollisionResult DetectCollisions(CollisionDetectionParameters parameters)
        {
            var result = new CollisionResult();

            // Broad Phase : AABB collision with BVH Tree
            var movedBodyRect3D = parameters.MovableBody.GetBounds(parameters.MovableBody.Position + parameters.MoveDirection);
            var collidedObjects = parameters.MapBvhTree.GetIntersected(movedBodyRect3D);

            if (!collidedObjects.Any())
            {
                return result;
            }

            // Near Phase : Separate Axis Theorem
            var manager = new SATCollisionManager();
            var movedBodyTriangles = Triangle3DHelper.CreateTriangles(movedBodyRect3D);

            var moveDirection = new Vector3D(parameters.MoveDirection.X, parameters.MoveDirection.Y, parameters.MoveDirection.Z);
            moveDirection.Normalize();

            var objectCounter = 0;
            foreach (var obj in collidedObjects)
            {
                objectCounter++;
                var collidedObjectTriangles = CreateTrianglesAndTransform(obj.Model3D);
                var collisionResults = manager.DetectCollisionsWithInfos(movedBodyTriangles, collidedObjectTriangles);

                if (collisionResults.HasIntersect)
                {
                    result.Collisions.Add(new CollisionDetail
                    {
                        CollidedTriangles = collisionResults.Intersections,
                        CollidedObject = obj.Model3D
                    });
                }
            }

            return result;
        }

        public StairClimbingResult ComputeStairClimbing(CollisionDetectionParameters parameters, CollisionResult collisionResult)
        {
            var result = new StairClimbingResult();

            var collidedTriangles = collisionResult.Collisions.SelectMany(x => x.CollidedTriangles).ToList();

            var maxStairHeight = 1;

            var highestY = collidedTriangles.Max(x => x.GetHighestPoint().Y);

            var heightDifference = highestY - parameters.MovableBody.Position.Y;

            if (heightDifference > 0 && heightDifference <= maxStairHeight)
            {
                // Adjust move direction to simulate climbing
                var adjustedDirection = parameters.MoveDirection + new Vector3D(0, heightDifference + .1, 0);

                // Check if the adjusted move is free of collisions
                var adjustedMoveParameters = new CollisionDetectionParameters
                {
                    MapBvhTree = parameters.MapBvhTree,
                    MovableBody = parameters.MovableBody,
                    MoveDirection = adjustedDirection
                };

                var adjustedMoveResult = DetectCollisions(adjustedMoveParameters);

                if (!adjustedMoveResult.HasCollision)
                {
                    result.CanClimbStairs = true;
                    result.Direction = adjustedDirection;
                    return result;
                }
            }

            return result;
        }

        public WallSlidingResult ComputeWallSliding(CollisionDetectionParameters parameters, CollisionResult collisionResult)
        {
            var result = new WallSlidingResult();
            var collidedTriangles = collisionResult.Collisions.SelectMany(x => x.CollidedTriangles).ToList();
            var slidableTriangles = GetSlidableTriangles(collidedTriangles, parameters.MoveDirection);
            var nearestTriangle = Graphics.Geometry3D.SortTrianglesByNearest(slidableTriangles, parameters.MovableBody.Position).FirstOrDefault();

            var slidingDirection = parameters.MoveDirection;
            if (nearestTriangle != null)
            {
                var normal = nearestTriangle.FaceNormal;
                slidingDirection = slidingDirection - Vector3D.DotProduct(slidingDirection, normal) * normal;
            }

            if (slidingDirection != parameters.MoveDirection)
            {
                var nearestMoveParameters = new CollisionDetectionParameters
                {
                    MapBvhTree = parameters.MapBvhTree,
                    MovableBody = parameters.MovableBody,
                    MoveDirection = slidingDirection
                };

                var nearestMoveResult = DetectCollisions(nearestMoveParameters);

                if (!nearestMoveResult.HasCollision)
                {
                    result.CanWallSlide = true;
                    result.Direction = slidingDirection;
                }
            }

            return result;
        }

        private IEnumerable<Triangle3D> GetSlidableTriangles(IEnumerable<Triangle3D> triangles, Vector3D moveDirection)
        {
            var results = new List<Triangle3D>();
            foreach (var triangle in triangles)
            {
                var collisionAngle = Math.Round(Vector3D.AngleBetween(triangle.FaceNormal, moveDirection));

                if (collisionAngle < 90)
                {
                    results.Add(triangle);
                }
            }

            return results;
        }

        private List<Triangle3D> CreateTrianglesAndTransform(LayeredModel3D model)
        {
            var objectTriangles = Triangle3DHelper.CreateTriangles(model.Children);
            foreach (var triangle in objectTriangles)
            {
                model.Transform(triangle.Positions);
                triangle.ComputeNormalAndEdges();
            }

            return objectTriangles;
        }
    }
}
