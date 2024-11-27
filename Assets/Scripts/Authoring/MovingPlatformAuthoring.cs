using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
[System.Serializable]
public struct MovingPlatform : IComponentData
{
    public float3 TranslationAxis;
    public float TranslationAmplitude;
    public float TranslationSpeed;
    public float3 RotationAxis;
    public float RotationSpeed;

    [HideInInspector] public bool IsInitialized;
    [HideInInspector] public float3 Position;
    [HideInInspector] public float3 Rotation;
}

public class MovingPlatformAuthoring : MonoBehaviour
{
    [SerializeField] private MovingPlatform MovingPlatform;

    public class Baker : Baker<MovingPlatformAuthoring>
    {
        public override void Bake(MovingPlatformAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, authoring.MovingPlatform);
        }
    }
}


