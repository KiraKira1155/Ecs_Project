using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
partial struct MoveEnemySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Enemy>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new RotationJob
        {
            elapsedTime = SystemAPI.Time.DeltaTime
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
partial struct RotationJob : IJobEntity
{
    public float elapsedTime;
    [BurstCompile]
    public void Execute(Enemy enemy, ref LocalTransform transform)
    {
        transform.Rotation = math.mul(quaternion.AxisAngle(new float3(15, 30, 45), enemy.RotationSpeed * elapsedTime), math.normalize(transform.Rotation));
    }
}
