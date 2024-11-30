using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionSphere : IComponentData
{
    [HideInInspector] public LocalTransform Transform;
    [HideInInspector] public float Radius;
    [HideInInspector] public float DoubleRadius;
}

[RequireComponent(typeof(EntityCollisionAuthoring))]
[ExecuteInEditMode]
public class CollisionSphereAuthoring : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Vector3 center;

    private void Update()
    {
        Gizmos.DrawWireSphere(transform.position + center, radius);
    }

    public class Baker : Baker<CollisionSphereAuthoring>
    {
        public override void Bake(CollisionSphereAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionSphere
            {
                Radius = authoring.radius,
                DoubleRadius = authoring.radius * authoring.radius,
            });
        }
    }
}
