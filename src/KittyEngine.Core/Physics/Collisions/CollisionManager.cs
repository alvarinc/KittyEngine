﻿using KittyEngine.Core.Physics.Collisions.SAT;
using KittyEngine.Core.Graphics;
using System.Windows.Media.Media3D;
using KittyEngine.Core.Graphics.Models.Builders;
using KittyEngine.Core.Services.Logging;

namespace KittyEngine.Core.Physics.Collisions
{
    public class CollisionManager : ICollisionManager
    {
        private ILogger _logger;

        public CollisionManager(ILogger logger)
        {
            _logger = logger;
        }

        public CollisionResult DetectCollisions(CollisionDetectionParameters parameters)
        {
            var result = new CollisionResult();

            // Broad Phase : AABB collision with BVH Tree
            var movedBodyRect3D = parameters.RigidBody.GetBounds(parameters.RigidBody.Position + parameters.MoveDirection);
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
            _logger.Log(LogLevel.Info, $"Stair climbing?? collidedTriangles: {collidedTriangles.Count}");

            var movedBodyRect3D = parameters.RigidBody.GetBounds(parameters.RigidBody.Position + parameters.MoveDirection);

            var intersectionPoints = Intersections.GetIntersectionPoints(movedBodyRect3D, collidedTriangles);
            _logger.Log(LogLevel.Info, $"Stair climbing?? intersects: {intersectionPoints.Count}");

            var insideVertices = Intersections.GetVerticesInsideRect3D(movedBodyRect3D, collidedTriangles);
            _logger.Log(LogLevel.Info, $"Stair climbing?? insideVertices: {insideVertices.Count}");

            var points = intersectionPoints.Union(insideVertices).Distinct().ToList();

            foreach (var point in points)
            {
                _logger.Log(LogLevel.Info, $"Stair climbing?? point: {point}");
            }

            var maxStairHeight = 1;

            var highestY = parameters.RigidBody.Position.Y;

            if (points.Count > 0)
            {
                highestY = points.Max(x => x.Y);
            }

            var heightDifference = highestY - parameters.RigidBody.Position.Y;

            _logger.Log(LogLevel.Info, $"Stair climbing??: {heightDifference} points: {points.Count}");
            if (heightDifference > 0 && heightDifference <= maxStairHeight)
            {
                _logger.Log(LogLevel.Info, $"Stair climbing: {heightDifference}");

                // Adjust move direction to simulate climbing
                var adjustedDirection = parameters.MoveDirection + new Vector3D(0, heightDifference + .1, 0);

                // Check if the adjusted move is free of collisions
                var adjustedMoveParameters = new CollisionDetectionParameters
                {
                    MapBvhTree = parameters.MapBvhTree,
                    RigidBody = parameters.RigidBody,
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
            var collidedTriangles = collisionResult.Collisions.SelectMany(x => x.CollidedTriangles).ToList();
            var slidableTriangles = collidedTriangles.Where(x => IsSlidableTriangle(x, parameters.MoveDirection));
            var nearestTriangles = Graphics.Geometry3D.SortTrianglesByNearest(slidableTriangles, parameters.RigidBody.Position);

            var alternateSlidingDirection1 = parameters.MoveDirection;
            var alternateSlidingDirection2 = parameters.MoveDirection;
            foreach (var triangle in slidableTriangles)
            {
                var normal = triangle.FaceNormal;
                alternateSlidingDirection1 = alternateSlidingDirection1 - Vector3D.DotProduct(alternateSlidingDirection1, normal) * normal;

                if (alternateSlidingDirection2 == parameters.MoveDirection)
                {
                    alternateSlidingDirection2 = alternateSlidingDirection1;
                }
            }

            var result = CanWallSlide(parameters, alternateSlidingDirection1);
            if (!result.CanWallSlide)
            {
                result = CanWallSlide(parameters, alternateSlidingDirection2);
            }

            return result;
        }

        private WallSlidingResult CanWallSlide(CollisionDetectionParameters parameters, Vector3D alternateSlidingDirection1)
        {
            var result = new WallSlidingResult();
            var slidingAngle = Vector3D.AngleBetween(parameters.MoveDirection, alternateSlidingDirection1);
            if (alternateSlidingDirection1 != parameters.MoveDirection && slidingAngle <= 90)
            {
                var nearestMoveParameters = new CollisionDetectionParameters
                {
                    MapBvhTree = parameters.MapBvhTree,
                    RigidBody = parameters.RigidBody,
                    MoveDirection = alternateSlidingDirection1
                };

                var nearestMoveResult = DetectCollisions(nearestMoveParameters);

                if (!nearestMoveResult.HasCollision)
                {
                    result.CanWallSlide = true;
                    result.Direction = alternateSlidingDirection1;
                }
            }

            return result;
        }

        private bool IsSlidableTriangle(Triangle3D triangle, Vector3D moveDirection)
        {
            var collisionAngle = Math.Round(Vector3D.AngleBetween(triangle.FaceNormal, moveDirection));
            return collisionAngle < 90;
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
