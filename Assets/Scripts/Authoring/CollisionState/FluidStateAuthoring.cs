using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct FluidState : IComponentData
{
    [ReadOnly]
    public float WaterSurfacePos;
}

public class FluidStateAuthoring : MonoBehaviour
{
    public class Baker : Baker<FluidStateAuthoring>
    {
        public override void Bake(FluidStateAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent<FluidState>(entity);
        }
    }
}
