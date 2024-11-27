using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct GravityEffect : IComponentData
{
    [Header("¿—Ê")]
    [Tooltip("ƒLƒƒOƒ‰ƒ€")]
    public float Mass;
    [Header("‹ó‹C’ïRŒW”")]
    public float AirResistanceCoefficient;

    [HideInInspector] public float CurrentSpeed;
    [HideInInspector] public float FallDistance;
    [HideInInspector] public float FallTime;
    [HideInInspector] public bool OnGround;
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
