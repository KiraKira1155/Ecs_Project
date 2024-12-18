using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(GravitySystem))]
partial struct EntityCollisionMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var time = SystemAPI.Time.DeltaTime;
        foreach (var (transform, collision) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EntityCollision>>())
        {
            transform.ValueRW.Position += collision.ValueRO.GetForce() * time;
            collision.ValueRW.MoveSpeed = collision.ValueRO.GetForce();
            collision.ValueRW.PreviousForce = collision.ValueRO.GetForce() * time;

            collision.ValueRW.ResetForce();
        }
    }
}
