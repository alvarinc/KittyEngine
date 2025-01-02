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
            var result = new CollisionResult { HasCollision = false };

            // Broad Phase : AABB collision with BVH Tree
            var objectBounds = parameters.MovableBody.GetBounds(parameters.MovableBody.Position + parameters.MoveDirection);
            var collidedObjects = parameters.BvhTree.GetIntersected(objectBounds);

            if (!collidedObjects.Any())
            {
                return result;
            }

            var hasCollistion = false;
            var manager = new SATCollisionManager();
            var objectTriangles = Triangle3DHelper.CreateTriangles(objectBounds);

            var moveDirection = new Vector3D(parameters.MoveDirection.X, parameters.MoveDirection.Y, parameters.MoveDirection.Z);
            moveDirection.Normalize();

            var collidedTriangles = new List<Triangle3D>();
            var objectCounter = 0;
            foreach (var obj in collidedObjects)
            {
                objectCounter++;
                var collidedObjectTriangles = CreateTrianglesAndTransform(obj.Model3D);
                var collisionResults = manager.DetectCollisionsWithInfos(objectTriangles, collidedObjectTriangles);

                if (collisionResults.HasIntersect)
                {
                    hasCollistion |= true;
                    collidedTriangles.AddRange(collisionResults.Intersections);
                }
            }

            result.HasCollision = hasCollistion;
            
            if (hasCollistion)
            {
                var objectOriginBounds = parameters.MovableBody.GetBounds(parameters.MovableBody.Position);
                var objectOriginTriangles = Triangle3DHelper.CreateTriangles(objectOriginBounds);
                var nearestDistanceManager = new SATNearestPointManager();
                var nearestDistanceResult = nearestDistanceManager.CalculateNearestDistance(objectOriginTriangles, collidedTriangles, parameters.MoveDirection);

                if (nearestDistanceResult.NearestDistance > 0)
                {
                    result.NearestMoveComputed = true;
                    result.NearestMove = nearestDistanceResult.NearestMove;
                }
            }

            return result;
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
