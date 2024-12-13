using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[System.Serializable]
[MaterialProperty("_PlayerColor")]
[BurstCompile]
public struct PlayerColor : IComponentData
{
    public float3 Value;
}

[System.Serializable]
[MaterialProperty("_PlayerAlphaColor")]
[BurstCompile]
public struct PlayerAlphaColor : IComponentData
{
    public float Value;
}

public class PlayerMaterialAuthoring : BaseColor
{
    public class Baker : Baker<PlayerMaterialAuthoring>
    {
        public override void Bake(PlayerMaterialAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerColor
            {
                Value = authoring.color
            });
            AddComponent(entity, new PlayerAlphaColor
            {
                Value = authoring.alpha
            });
        }
    }
}
