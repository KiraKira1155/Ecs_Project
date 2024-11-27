using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Spawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        foreach (var spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            spawner.ValueRW.NextSpawnTime += SystemAPI.Time.DeltaTime;


            if (spawner.ValueRO.NextSpawnTime > spawner.ValueRO.SpawnInterval)
            {

                var instances = state.EntityManager.Instantiate
                    (spawner.ValueRO.Entity, spawner.ValueRO.SpawnAmount, Unity.Collections.Allocator.Temp);

                spawner.ValueRW.NextSpawnTime = 0;

                foreach (var entity in instances)
                {
                    var random = Unity.Mathematics.Random.CreateFromIndex((uint)spawner.ValueRO.EnemyAmount);
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);

                    var pos = random.NextFloat3(spawner.ValueRO.MinSpawnPos, spawner.ValueRO.MaxSpawnPos);

                    transform.ValueRW = LocalTransform.FromPosition(pos);

                    spawner.ValueRW.EnemyAmount++;
                }

            }
        }
    }
}
