using Unity.Burst;
using Unity.Entities;
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
