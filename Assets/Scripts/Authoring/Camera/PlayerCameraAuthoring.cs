using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct PlayerCamera : IComponentData
{
}

[DisallowMultipleComponent]
public class PlayerCameraAuthoring : MonoBehaviour
{
    public class Baker : Baker<PlayerCameraAuthoring>
    {
        public override void Bake(PlayerCameraAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerCamera>(entity);
        }
    }
}
