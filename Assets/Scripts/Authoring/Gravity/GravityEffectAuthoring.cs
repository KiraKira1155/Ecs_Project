using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct GravityEffect : IComponentData
{
    [Header("����")]
    [Tooltip("�L���O����")]
    public float Mass;
    [Header("��C��R�W��")]
    public float AirResistanceCoefficient;

    [HideInInspector] public float CurrentSpeed;
    [HideInInspector] public float FallDistance;
    [HideInInspector] public float FallTime;
}

public class GravityEffectAuthoring : MonoBehaviour
{
    [SerializeField] private GravityEffect gravityEffect;

    public class Baker : Baker<GravityEffectAuthoring>
    {
        public override void Bake(GravityEffectAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, authoring.gravityEffect);
        }
    }
}
