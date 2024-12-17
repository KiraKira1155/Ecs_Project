using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
partial struct EntityCollisionSettingSystem : ISystem
{
    bool init;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        init = false;

        //new EntityShapeCollisionInitSettingJob
        //{
        //    collisionGroup = SystemAPI.GetComponentLookup<EntityCollision>(),
        //    aabbGroup = SystemAPI.GetComponentLookup<CollisionAABB>(),
        //    pillarGroup = SystemAPI.GetComponentLookup<CollisionPillar>(),
        //    sphereGroup = SystemAPI.GetComponentLookup<CollisionSphere>()
        //}.ScheduleParallel();
    }

    [BurstDiscard]
    private void DebugInfo(uint id)
    {
        Debug.LogWarning(id);
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!init)
        {
            var id = 1;

            foreach (var (collision, transform) in SystemAPI.Query<RefRW<EntityCollision>, RefRO<LocalTransform>>())
            {
                if (!collision.ValueRO.HasParentEntity)
                {
                    collision.ValueRW.EntityLocalPos = transform.ValueRO.Position;
                    collision.ValueRW.ID = id++;
                    collision.ValueRW.BeInitSetting = true;
                }
            }

            foreach(var (collision, transform) in SystemAPI.Query<RefRW<EntityCollision>, RefRO<LocalTransform>>())
            {
                foreach(var parentCollision in SystemAPI.Query<RefRO<EntityCollision>>())
                {
                    if(parentCollision.ValueRO.ObjectID == collision.ValueRO.ObjectID)
                    {
                        collision.ValueRW.EntityLocalPos = transform.ValueRO.Position;
                        collision.ValueRW.ID = parentCollision.ValueRO.ID;
                        collision.ValueRW.BeInitSetting = true;
                    }
                }
            }

            init = true;
        }
        else
        {
            foreach (var (collision, transform) in SystemAPI.Query<RefRW<EntityCollision>, RefRO<LocalTransform>>())
            {
                if (!collision.ValueRO.IsFreeze)
                {
                    collision.ValueRW.EntityLocalPos = transform.ValueRO.Position;
                }
            }
        }
    }
}

[BurstCompile]
partial struct EntityCollisionSettingJob : IJobEntity
{
    [BurstCompile]
    public void Execute(ref LocalTransform transform, ref EntityCollision collision)
    {
        if (!collision.IsFreeze && collision.BeInitSetting)
        {
            collision.EntityLocalPos = transform.Position;
        }
        else if(!collision.BeInitSetting)
        {
            collision.EntityLocalPos = transform.Position;
            collision.BeInitSetting = true;
        }
    }
}

//[BurstCompile]
//partial struct EntityShapeCollisionInitSettingJob : IJobEntity
//{
//    [ReadOnly] public ComponentLookup<EntityCollision> collisionGroup;
//    [ReadOnly] public ComponentLookup<CollisionAABB> aabbGroup;
//    [ReadOnly] public ComponentLookup<CollisionPillar> pillarGroup;
//    [ReadOnly] public ComponentLookup<CollisionSphere> sphereGroup;

//    private const byte COLLISION_TYPE_AABB = 0x0001;
//    private const byte COLLISION_TYPE_PILLAR = 0x0002;
//    private const byte COLLISION_TYPE_SPHERE = 0x0004;

//    [BurstCompile]
//    public void Execute(ref EntityCollision collision)
//    {
//        byte collisionCheck = 0x0000;
//        if (aabbGroup.HasComponent(collision.Entity))
//        {
//            collisionCheck |= COLLISION_TYPE_AABB;
//        }
//        if (pillarGroup.HasComponent(collision.Entity))
//        {
//            collisionCheck |= COLLISION_TYPE_PILLAR;
//        }
//        if (sphereGroup.HasComponent(collision.Entity))
//        {
//            collisionCheck |= COLLISION_TYPE_SPHERE;
//        }

//        switch (collisionCheck)
//        {
//            case 0x0000:
//                break;

//            case COLLISION_TYPE_AABB:
//                var aabb = aabbGroup[collision.Entity];
//                if (!aabb.HasParentEntity)
//                {
//                    collision.ID = aabb.ID;
//                }
//                else
//                {
//                }
//                break;

//            case COLLISION_TYPE_PILLAR:
//                var pillar = pillarGroup[collision.Entity];
//                if (!pillar.HasParentEntity)
//                {
//                    collision.ID = pillar.ID;
//                }
//                else
//                {
//                }
//                break;

//            case COLLISION_TYPE_SPHERE:
//                var sphere = sphereGroup[collision.Entity];
//                if (!sphere.HasParentEntity)
//                {
//                    collision.ID = sphere.ID;
//                }
//                else
//                {
//                }
//                break;

//            default:
//                DebugInfo(collision.Entity);
//                break;
//        }


//    }

//    [BurstCompile]
//    private uint GetParentID()
//    {
//        return 0;
//    }

//    [BurstDiscard]
//    private void DebugInfo(Entity entity)
//    {
//        Debug.LogWarning("Entity = " + entity +" : ���Entity�ɑ΂��ĕ����̓����蔻��͔F�߂��Ă��Ȃ�");
//    }
//}
