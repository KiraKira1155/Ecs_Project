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
    [Header("����")]
    [Tooltip("kg")]
    public float Mass;
    
    [Header("�f�ʐ�")]
    [Tooltip("m^2")]
    public float CrossSection;

    [Header("���̂̍R�͌W��")]
    public float Cd;

    [Header("���x")]
    [Tooltip("kg/m^3")]
    public float Density;

    [Header("�g��")]
    [Tooltip("���[�g��")]
    public float Height;

    [Header("�Z�o�����l")]
    [ReadOnly]
    public float FallTime;

    [ReadOnly]
    public float TerminalVelocity;

    [ReadOnly]
    public float TerminalVelocityWater;


    [HideInInspector]
    public float ResistanceAir; // �R�͌W��

    [HideInInspector]
    public float ResistanceWater; // �R�͌W��

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
