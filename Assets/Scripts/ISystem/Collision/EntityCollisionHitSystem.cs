using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(EntityCollisionSettingSystem))]
partial struct EntityCollisionHitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CollisionSphere>();
        state.RequireForUpdate<CollisionAABB>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // aabb VS
        foreach(var (collision, aabb) in SystemAPI.Query<RefRW<EntityCollision>, RefRW<CollisionAABB>>())
        {
            if (collision.ValueRO.IsFreeze)
                continue;

            var hit = false;

            aabb.ValueRW.AABB.EntityCenterPos = collision.ValueRO.EntityLocalPos;

            
            var hitPos = float3.zero;

            foreach (var (defense, d_sphere) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionSphere>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_sphere.ValueRW.Sphere.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.AABBVSSphere(aabb.ValueRO.AABB, d_sphere.ValueRO.Sphere, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }
            foreach (var (defense, d_aabb) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionAABB>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_aabb.ValueRW.AABB.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.AABBVSAABB(aabb.ValueRO.AABB, d_aabb.ValueRO.AABB, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }
            foreach (var (defense, d_pillar) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionPillar>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_pillar.ValueRW.Pillar.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.AABBVSPillar(aabb.ValueRO.AABB, d_pillar.ValueRO.Pillar, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }

            if (hit && SystemAPI.HasComponent<GravityEffect>(collision.ValueRO.Entity))
            {
                collision.ValueRW.OnGround = true;
            }
            else if (!hit)
            {
                collision.ValueRW.OnGround = false;
            }
        }


        // sphere VS
        foreach (var (collision, sphere) in SystemAPI.Query<RefRW<EntityCollision>, RefRW<CollisionSphere>>())
        {
            if (collision.ValueRO.IsFreeze)
                continue;

            var hit = false;

            sphere.ValueRW.Sphere.EntityCenterPos = collision.ValueRO.EntityLocalPos;

            var hitPos = float3.zero;

            foreach (var (defense, d_sphere) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionSphere>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_sphere.ValueRW.Sphere.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.SphereVSSphere(sphere.ValueRO.Sphere, d_sphere.ValueRO.Sphere, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }
            foreach (var (defense, d_aabb) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionAABB>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_aabb.ValueRW.AABB.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.AABBVSSphere(d_aabb.ValueRO.AABB, sphere.ValueRO.Sphere, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }
            foreach (var (defense, d_pillar) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionPillar>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_pillar.ValueRW.Pillar.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.SphereVSPillar(sphere.ValueRO.Sphere, d_pillar.ValueRO.Pillar, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }

            if (hit && SystemAPI.HasComponent<GravityEffect>(collision.ValueRO.Entity))
            {
                collision.ValueRW.OnGround = true;
            }
            else if (!hit)
            {
                collision.ValueRW.OnGround = false;
            }
        }


        // pillar VS
        foreach (var (collision, pillar) in SystemAPI.Query<RefRW<EntityCollision>, RefRW<CollisionPillar>>())
        {
            if (collision.ValueRO.IsFreeze)
                continue;

            var hit = false;

            pillar.ValueRW.Pillar.EntityCenterPos = collision.ValueRO.EntityLocalPos;

            var hitPos = float3.zero;

            foreach (var (defense, d_sphere) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionSphere>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_sphere.ValueRW.Sphere.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.SphereVSPillar(d_sphere.ValueRO.Sphere, pillar.ValueRO.Pillar, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }
            foreach (var (defense, d_aabb) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionAABB>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_aabb.ValueRW.AABB.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.AABBVSPillar(d_aabb.ValueRO.AABB, pillar.ValueRO.Pillar, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }
            foreach (var (defense, d_pillar) in SystemAPI.Query<RefRO<EntityCollision>, RefRW<CollisionPillar>>())
            {
                if (collision.ValueRO.ID == defense.ValueRO.ID)
                    continue;

                d_pillar.ValueRW.Pillar.EntityCenterPos = defense.ValueRO.EntityLocalPos;
                if (HitCalculationUtilities.PillarVSPillar(pillar.ValueRO.Pillar, d_pillar.ValueRO.Pillar, collision.ValueRO.CanCheckHitPos, out hitPos))
                {
                    hit = true;
                }
            }

            if (hit && SystemAPI.HasComponent<GravityEffect>(collision.ValueRO.Entity))
            {
                collision.ValueRW.OnGround = true;
            }
            else if (!hit)
            {
                collision.ValueRW.OnGround = false;
            }
        }
    }
}
