using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;

public static class ShapeManager
{
    public enum ShapeType
    {
        None = 0,
        Circle,
        Quad,
        Sphere,
        AABB,
        Pillar
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 QuadRect(in float2 halfSize, in float2 center, in float2 entityPos)
    {
        return new float4
        {
            x = center.x - halfSize.x + entityPos.x,
            y = center.y - halfSize.y + entityPos.y,
            z = center.x + halfSize.x + entityPos.x,
            w = center.y + halfSize.y + entityPos.y
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 EntityCenterPos2D(in float3 entityPos, in HitCalculationUtilities.ColliderDirection direction)
    {
        switch (direction)
        {
            case HitCalculationUtilities.ColliderDirection.XY:
                return new float2
                {
                    x = entityPos.x,
                    y = entityPos.y,
                };

            case HitCalculationUtilities.ColliderDirection.XZ:
                return new float2
                {
                    x = entityPos.x,
                    y = entityPos.z,
                };

            default:
                return new float2
                {
                    x = entityPos.y,
                    y = entityPos.z,
                };
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static QuadShape SetQuad(in float3 entityPos, in float2 center, in float2 size, in HitCalculationUtilities.ColliderDirection direction)
    {
        var halfSize = size * 0.5f;
        return new QuadShape
        {
            ShapeCenterPos = center,
            EntityCenterPos = EntityCenterPos2D(entityPos, direction),
            Size = size,
            HalfSize = halfSize,
            Rect = QuadRect(halfSize, center, EntityCenterPos2D(entityPos, direction)),
            CollisionDirection = direction
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CircleShape SetCircle(in float3 entityPos, in float2 center, in float radius, in HitCalculationUtilities.ColliderDirection direction)
    {
        return new CircleShape
        {
            ShapeCenterPos = center,
            EntityCenterPos = EntityCenterPos2D(entityPos, direction),
            Radius = radius,
            DoubleRadius = radius * radius,
            CollisionDirection = direction
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SphereShape SetSphere(in float3 entityPos, in float3 center, in float radius)
    {
        return new SphereShape
        {
            ShapeCenterPos = center,
            EntityCenterPos = entityPos,
            Radius = radius,
            DoubleRadius = radius * radius
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AABBShape SetAABB(in float3 entityPos, in float3 center, in float3 size)
    {
        var halfSize = size * 0.5f;
        return new AABBShape
        {
            ShapeCenterPos = center,
            EntityCenterPos = entityPos,
            Size = size,
            HalfSize = halfSize,
            MinPos = center - halfSize,
            MaxPos = center + halfSize,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PillarShape SetPillar(in float3 entityPos, in float3 center, in float radius, in float height)
    {
        var circle = SetCircle(entityPos, new float2(center.x, center.z), radius, HitCalculationUtilities.ColliderDirection.XZ);
        return new PillarShape
        {
            Circle = circle,
            ShapeCenterPos = center,
            EntityCenterPos= entityPos,
            Height = height,
            BottomPosY = center.y - height * 0.5f,
            TopPosY = center.y + height * 0.5f
        };
    }
}
[System.Serializable]
[BurstCompile]
public struct QuadShape
{
    public float2 EntityCenterPos;

    public float2 ShapeCenterPos;

    public float2 Size;

    public float2 HalfSize;

    public float4 Rect;

    public HitCalculationUtilities.ColliderDirection CollisionDirection;
}

[System.Serializable]
[BurstCompile]
public struct CircleShape
{
    public float2 EntityCenterPos;

    public float2 ShapeCenterPos;

    public float Radius;

    public float DoubleRadius;

    public HitCalculationUtilities.ColliderDirection CollisionDirection;
}

[System.Serializable]
[BurstCompile]
public struct SphereShape
{
    public float3 EntityCenterPos;

    public float3 ShapeCenterPos;

    public float Radius;

    public float DoubleRadius;
}

[System.Serializable]
[BurstCompile]
public struct AABBShape
{
    public float3 EntityCenterPos;

    public float3 ShapeCenterPos;

    public float3 Size;

    public float3 HalfSize;

    public float3 MinPos;

    public float3 MaxPos;
}

[System.Serializable]
[BurstCompile]
public struct PillarShape
{
    public CircleShape Circle;

    public float3 EntityCenterPos;

    public float3 ShapeCenterPos;

    public float Height;

    public float BottomPosY;

    public float TopPosY;
}

