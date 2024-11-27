using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionSphere : IComponentData
{
    [HideInInspector] public float Radius;
    [HideInInspector] public float DoubleRadius;
    [HideInInspector] public float3 CenterPos;
}

public class CollisionSphereAuthoring : MonoBehaviour
{
    [SerializeField] private float radius;
    public class Baker : Baker<CollisionSphereAuthoring>
    {
        public override void Bake(CollisionSphereAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionSphere
            {
                Radius = authoring.radius,
                DoubleRadius = authoring.radius * authoring.radius
            });
        }
    }
}
