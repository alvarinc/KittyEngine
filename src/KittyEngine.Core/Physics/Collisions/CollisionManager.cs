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
            var collidedObjects = parameters.BvhTree.GetIntersected(parameters.ObjectBounds);

            if (!collidedObjects.Any())
            {
                return result;
            }

            var hasCollistion = false;
            var manager = new SATCollisionManager();
            var objectTriangles = Triangle3DHelper.CreateTriangles(parameters.ObjectBounds);

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

                    foreach (var triangle in collisionResults.Intersections)
                    {
                        var faceNormal = triangle.FaceNormal;
                        var collisionAngle = Vector3D.AngleBetween(faceNormal, moveDirection);
                        var normalAngle = Vector3D.AngleBetween(faceNormal, new Vector3D(0, 1, 0));
                        var nearestPointOnTriangle = Graphics.Geometry3D.NearestPointOnTriangle(triangle, parameters.Origin);
                        var distance = (parameters.Origin - nearestPointOnTriangle).Length;
                    }
                }
            }

            result.HasCollision = hasCollistion;
            
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
