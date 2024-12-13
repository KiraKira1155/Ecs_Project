using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct GravityEffect : IComponentData
{
    [Header("éøó ")]
    [Tooltip("kg")]
    public float Mass;
    
    [Header("ífñ êœ")]
    [Tooltip("m^2")]
    public float CrossSection;

    [Header("ï®ëÃÇÃçRóÕåWêî")]
    public float Cd;

    [Header("ñßìx")]
    [Tooltip("kg/m^3")]
    public float Density;

    [Header("êgí∑")]
    [Tooltip("ÉÅÅ[ÉgÉã")]
    public float Height;

    [Header("éZèoÇ≥ÇÍÇÈíl")]
    [ReadOnly]
    public float FallTime;

    [ReadOnly]
    public float TerminalVelocity;

    [ReadOnly]
    public float TerminalVelocityWater;


    [HideInInspector]
    public float ResistanceAir; // çRóÕåWêî

    [HideInInspector]
    public float ResistanceWater; // çRóÕåWêî

    [HideInInspector]
    public bool IsUnderwater;

    [HideInInspector]
    public float DistanceFromWaterSurface;
}

public class GravityEffectAuthoring : MonoBehaviour
{
    [SerializeField] private GravityEffect gravityEffect;

    public class Baker : Baker<GravityEffectAuthoring>
    {
        public override void Bake(GravityEffectAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            authoring.gravityEffect.ResistanceAir = authoring.gravityEffect.Cd * authoring.gravityEffect.CrossSection * PhysicsConstantUtility.Air * 0.5f;
            authoring.gravityEffect.ResistanceWater = authoring.gravityEffect.Cd * authoring.gravityEffect.CrossSection * PhysicsConstantUtility.Water * 0.5f;

            authoring.gravityEffect.TerminalVelocity = Mathf.Sqrt((2* authoring.gravityEffect.Mass*PhysicsConstantUtility.G) / (PhysicsConstantUtility.Air * authoring.gravityEffect.CrossSection * authoring.gravityEffect.Cd));
            authoring.gravityEffect.TerminalVelocityWater = Mathf.Sqrt((2 * authoring.gravityEffect.Mass * PhysicsConstantUtility.G) / (PhysicsConstantUtility.Water * authoring.gravityEffect.CrossSection * authoring.gravityEffect.Cd));
            AddComponent(entity, authoring.gravityEffect);
        }
    }
}
