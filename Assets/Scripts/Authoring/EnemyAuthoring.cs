using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public struct Enemy : IComponentData
{
    public float RotationSpeed;
}

public class EnemyAuthoring : MonoBehaviour
{
    public float speed;


    public class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var data = new Enemy()
            {
            };

            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}
