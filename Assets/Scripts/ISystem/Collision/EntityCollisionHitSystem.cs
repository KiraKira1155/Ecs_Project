using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(EntityCollisionSettingSystem))]
partial struct EntityCollisionHitSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

    }
}

[BurstCompile]
partial struct EntityCollisionHitJob : IJobEntity
{
    [ReadOnly] public ComponentLookup<CollisionAABB> aabbGroup;
    [ReadOnly] public ComponentLookup<CollisionPillar> pillarGroup;
    [ReadOnly] public ComponentLookup<CollisionSphere> sphereGroup;

    private const byte COLLISION_TYPE_AABB = 0x0001;
    private const byte COLLISION_TYPE_PILLAR = 0x0002;
    private const byte COLLISION_TYPE_SPHERE = 0x0004;

    [BurstCompile]
    public void Execute(ref EntityCollision collision)
    {
        CheckCollisionType(collision);
    }

    [BurstCompile]
    private void HitCheck(in ShapeManager.ShapeType shapeType)
    {

    }

    [BurstCompile]
    private void CheckCollisionType(in EntityCollision collision)
    {
        byte collisionCheck = 0x0000;
        if (aabbGroup.HasComponent(collision.Entity))
        {
            collisionCheck |= COLLISION_TYPE_AABB;
        }
        if (pillarGroup.HasComponent(collision.Entity))
        {
            collisionCheck |= COLLISION_TYPE_PILLAR;
        }
        if (sphereGroup.HasComponent(collision.Entity))
        {
            collisionCheck |= COLLISION_TYPE_SPHERE;
        }
    }

    [BurstDiscard]
    private void DebugInfo()
    {
        Debug.LogWarning("àÍÇ¬ÇÃEntityÇ…ëŒÇµÇƒï°êîÇÃìñÇΩÇËîªíËÇÕîFÇﬂÇÁÇÍÇƒÇ¢Ç»Ç¢");
    }
}
