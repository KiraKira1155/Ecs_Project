using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public static class DebugDrawUtility
{
    private static Color defaultColor = Color.red;
    public static Color color = Color.red;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawSphere(in SphereShape _sphere, in float3 entityPos, int segments = 20)
    {
        DrawCircle(ShapeManager.SetCircle(entityPos, new float2(_sphere.ShapeCenterPos.x, _sphere.ShapeCenterPos.y), _sphere.Radius, HitCalculationUtilities.ColliderDirection.XY), entityPos, segments);
        DrawCircle(ShapeManager.SetCircle(entityPos, new float2(_sphere.ShapeCenterPos.y, _sphere.ShapeCenterPos.z), _sphere.Radius, HitCalculationUtilities.ColliderDirection.YZ), entityPos, segments);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawPillar(in PillarShape _pillar, in float3 entityPos, int segments = 20)
    {
        var halfHeight = new float3(0, _pillar.Height * 0.5f, 0);

        var pillar = new Pillar()
        {
            pillar = _pillar,
            entityPos = entityPos,
            angle = 0,
        };

        var angleStep = 360.0f / segments;
        var previousPoint = pillar.DrawPoint();
        var newPoint = float3.zero;

        for (int i = 0; i < segments; i++)
        {
            pillar.angle += angleStep;
            newPoint = pillar.DrawPoint();

            Debug.DrawLine(previousPoint + halfHeight, newPoint + halfHeight, color);
            Debug.DrawLine(previousPoint - halfHeight, newPoint - halfHeight, color);

            previousPoint = newPoint;
        }

        pillar.angle = 0.0f;
        Debug.DrawLine(pillar.DrawPoint() + halfHeight, pillar.DrawPoint() - halfHeight, color);
        pillar.angle = 90.0f;
        Debug.DrawLine(pillar.DrawPoint() + halfHeight, pillar.DrawPoint() - halfHeight, color);
        pillar.angle = 180.0f;
        Debug.DrawLine(pillar.DrawPoint() + halfHeight, pillar.DrawPoint() - halfHeight, color);
        pillar.angle = 270.0f;
        Debug.DrawLine(pillar.DrawPoint() + halfHeight, pillar.DrawPoint() - halfHeight, color);

        color = defaultColor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawCircle(in CircleShape _circle, in float3 entityPos, int segments = 20)
    {
        var circle = new Circle()
        {
            circle = _circle,
            entityPos = entityPos,
            angle = 0,
        };

        var angleStep = 360.0f / segments;
        var previousPoint = circle.DrawPoint();
        var newPoint = float3.zero;

        for(int i = 0; i < segments; i++)
        {
            circle.angle += angleStep;
            newPoint = circle.DrawPoint();

            Debug.DrawLine(previousPoint, newPoint, color);

            previousPoint = newPoint;
        }

        color = defaultColor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DrawAABB(in AABBShape _aabb, in float3 entityPos)
    {
        var height = new float3(0, _aabb.HalfSize.y, 0);
        var aabb = new AABB()
        {
            aabb = _aabb,
            entityPos = entityPos
        };

        // XZÀ•W•½–Ê
        var leftUp = aabb.LeftUp();
        var leftDown = aabb.LeftDown();
        var rightDown = aabb.RightDown();
        var rightUp = aabb.RightUp();

        // ‰º
        Debug.DrawLine(leftUp - height, leftDown - height, color);
        Debug.DrawLine(leftDown - height, rightDown - height, color);
        Debug.DrawLine(rightDown - height, rightUp - height, color);
        Debug.DrawLine(rightUp - height, leftUp - height, color);
        // ã
        Debug.DrawLine(leftUp + height, leftDown + height, color);
        Debug.DrawLine(leftDown + height, rightDown + height, color);
        Debug.DrawLine(rightDown + height, rightUp + height, color);
        Debug.DrawLine(rightUp + height, leftUp + height, color);
        // c
        Debug.DrawLine(leftUp + height, leftUp - height, color);
        Debug.DrawLine(leftDown + height, leftDown - height, color);
        Debug.DrawLine(rightUp + height, rightUp - height, color);
        Debug.DrawLine(rightDown + height, rightDown - height, color);

        color = defaultColor;
    }

    [BurstCompile]
    private struct Circle
    {
        public CircleShape circle;
        public float3 entityPos;
        public float angle;

        [BurstCompile]
        public readonly float3 DrawPoint()
        {
            switch (circle.CollisionDirection)
            {
                case HitCalculationUtilities.ColliderDirection.XY:
                    return new float3(circle.ShapeCenterPos.x, circle.ShapeCenterPos.y, 0) + entityPos + new float3(Mathf.Cos(Mathf.Deg2Rad * angle) * circle.Radius, Mathf.Sin(Mathf.Deg2Rad * angle) * circle.Radius, 0);

                case HitCalculationUtilities.ColliderDirection.XZ:
                    return new float3(circle.ShapeCenterPos.x, 0, circle.ShapeCenterPos.y) + entityPos + new float3(Mathf.Cos(Mathf.Deg2Rad * angle) * circle.Radius, 0, Mathf.Sin(Mathf.Deg2Rad * angle) * circle.Radius);

                default:
                    return new float3(0, circle.ShapeCenterPos.x, circle.ShapeCenterPos.y) + entityPos + new float3(0, Mathf.Cos(Mathf.Deg2Rad * angle) * circle.Radius, Mathf.Sin(Mathf.Deg2Rad * angle) * circle.Radius);
            }
        }
    }

    [BurstCompile]
    private struct Pillar
    {
        public PillarShape pillar;
        public float3 entityPos;
        public float angle;

        [BurstCompile]
        public readonly float3 DrawPoint()
        {
            return pillar.ShapeCenterPos + entityPos + new float3(Mathf.Cos(Mathf.Deg2Rad * angle) * pillar.Circle.Radius, 0, Mathf.Sin(Mathf.Deg2Rad * angle) * pillar.Circle.Radius);
        }
    }

    [BurstCompile]
    private struct AABB
    {
        public AABBShape aabb;
        public float3 entityPos;

        [BurstCompile]
        public readonly float3 RightUp()
        {
            return entityPos + new float3(aabb.ShapeCenterPos.x + aabb.HalfSize.x, aabb.ShapeCenterPos.y, aabb.ShapeCenterPos.z + aabb.HalfSize.z);
        }                                                                                            
                                                                                                     
        [BurstCompile]                                                                               
        public readonly float3 RightDown()                                                           
        {                                                                                            
            return entityPos + new float3(aabb.ShapeCenterPos.x + aabb.HalfSize.x, aabb.ShapeCenterPos.y, aabb.ShapeCenterPos.z - aabb.HalfSize.z);
        }                                                                                            
                                                                                                     
        [BurstCompile]                                                                               
        public readonly float3 LeftUp()                                                              
        {                                                                                            
            return entityPos + new float3(aabb.ShapeCenterPos.x - aabb.HalfSize.x, aabb.ShapeCenterPos.y, aabb.ShapeCenterPos.z + aabb.HalfSize.z);
        }

        [BurstCompile]
        public readonly float3 LeftDown()
        {
            return entityPos + new float3(aabb.ShapeCenterPos.x - aabb.HalfSize.x, aabb.ShapeCenterPos.y, aabb.ShapeCenterPos.z - aabb.HalfSize.z);
        }
    }

    [BurstCompile]
    private struct Quad
    {
        public QuadShape quad;
        public float3 entityPos;

        [BurstCompile]
        public readonly float3 RightUp()
        {
            switch (quad.CollisionDirection)
            {
                case HitCalculationUtilities.ColliderDirection.XY:
                    return entityPos + new float3(quad.ShapeCenterPos.x + quad.HalfSize.x, quad.ShapeCenterPos.y + quad.HalfSize.y, 0);

                case HitCalculationUtilities.ColliderDirection.XZ:
                    return entityPos + new float3(quad.ShapeCenterPos.x + quad.HalfSize.x, 0, quad.ShapeCenterPos.y + quad.HalfSize.y);

                default:
                    return entityPos + new float3(0, quad.ShapeCenterPos.x + quad.HalfSize.x, quad.ShapeCenterPos.y + quad.HalfSize.y);
            }
        }

        [BurstCompile]
        public readonly float3 RightDown()
        {
            switch (quad.CollisionDirection)
            {
                case HitCalculationUtilities.ColliderDirection.XY:
                    return entityPos + new float3(quad.ShapeCenterPos.x + quad.HalfSize.x, quad.ShapeCenterPos.y - quad.HalfSize.y, 0);

                case HitCalculationUtilities.ColliderDirection.XZ:
                    return entityPos + new float3(quad.ShapeCenterPos.x + quad.HalfSize.x, 0, quad.ShapeCenterPos.y - quad.HalfSize.y);

                default:
                    return entityPos + new float3(0, quad.ShapeCenterPos.x + quad.HalfSize.x, quad.ShapeCenterPos.y - quad.HalfSize.y);
            }
        }

        [BurstCompile]
        public readonly float3 LeftUp()
        {
            switch (quad.CollisionDirection)
            {
                case HitCalculationUtilities.ColliderDirection.XY:
                    return entityPos + new float3(quad.ShapeCenterPos.x - quad.HalfSize.x, quad.ShapeCenterPos.y + quad.HalfSize.y, 0);

                case HitCalculationUtilities.ColliderDirection.XZ:
                    return entityPos + new float3(quad.ShapeCenterPos.x - quad.HalfSize.x, 0, quad.ShapeCenterPos.y + quad.HalfSize.y);

                default:
                    return entityPos + new float3(0, quad.ShapeCenterPos.x - quad.HalfSize.x, quad.ShapeCenterPos.y + quad.HalfSize.y);
            }
        }

        [BurstCompile]
        public readonly float3 LeftDown()
        {
            switch (quad.CollisionDirection)
            {
                case HitCalculationUtilities.ColliderDirection.XY:
                    return entityPos + new float3(quad.ShapeCenterPos.x - quad.HalfSize.x, quad.ShapeCenterPos.y - quad.HalfSize.y, 0);

                case HitCalculationUtilities.ColliderDirection.XZ:
                    return entityPos + new float3(quad.ShapeCenterPos.x - quad.HalfSize.x, 0, quad.ShapeCenterPos.y - quad.HalfSize.y);

                default:
                    return entityPos + new float3(0, quad.ShapeCenterPos.x - quad.HalfSize.x, quad.ShapeCenterPos.y - quad.HalfSize.y);
            }
        }
    }
}


