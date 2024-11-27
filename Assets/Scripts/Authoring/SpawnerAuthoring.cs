using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
[System.Serializable]
public struct Spawner : IComponentData
{
    public int SpawnAmount;
    public float SpawnInterval;
    [Header("スポーンポジション設定")]
    public float3 MinSpawnPos;
    public float3 MaxSpawnPos;

    [HideInInspector] public int EnemyAmount;
    [HideInInspector] public float NextSpawnTime;
    [HideInInspector] public Entity Entity;
}


public class SpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Spawner spawner;

    public class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var spawnTerget = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic);
            authoring.spawner.Entity = spawnTerget;
            AddComponent(entity, authoring.spawner);
        }
    }
}
