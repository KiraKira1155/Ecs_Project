using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
partial struct EntityCollisionSettingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new EntityCollisionSettingJob { }.ScheduleParallel();

    }
}

[BurstCompile]
partial struct EntityCollisionSettingJob : IJobEntity
{
    [BurstCompile]
    public void Execute(ref LocalTransform transform, ref EntityCollision collision)
    {
        if (!collision.IsFreeze)
        {
            collision.CenterPos = transform.Position;
        }
        else
        {
            if (!collision.BeInitSetting)
            {
                collision.CenterPos = transform.Position;
                collision.BeInitSetting = true;
            }
        }
    }
}
