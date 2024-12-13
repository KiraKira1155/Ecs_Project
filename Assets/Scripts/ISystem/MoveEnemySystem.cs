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
        foreach (var (transform, collision) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EntityCollision>>())
        {
            transform.ValueRW.Position += collision.ValueRO.GetForce() * SystemAPI.Time.DeltaTime;
            collision.ValueRW.MoveSpeed = collision.ValueRO.GetForce();
            collision.ValueRW.PreviousForce = collision.ValueRO.GetForce();

            collision.ValueRW.ResetForce();
        }
    }
}
