using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

[BurstCompile]
public struct Blower : IComponentData
{
    public float3 Force;
}

public class BlowerAuthoring : MonoBehaviour
{
    public float3 force = math.float3(0, 100, 0);

    class Baker : Baker<BlowerAuthoring>
    {
        public override void Bake(BlowerAuthoring authoring)
        {
            var data = new Blower
            {
                Force = authoring.force
            };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}
