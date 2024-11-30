using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionAABB : IComponentData
{
    [HideInInspector] public float3 CenterPos;
    [HideInInspector] public float3 HalfSize;
    [HideInInspector] public float3 MinPos;
    [HideInInspector] public float3 MaxPos;


    [BurstCompile]
    public float3 GetMinPos()
    {
        MinPos = CenterPos - HalfSize;
        return MinPos;
    }

    [BurstCompile]
    public float3 GetMaxPos()
    {
        MaxPos = CenterPos + HalfSize;
        return MaxPos;
    }
}

[RequireComponent(typeof(EntityCollisionAuthoring))]
public class CollisionAABBAuthoring : MonoBehaviour
{
    [SerializeField] private float3 size;
    [SerializeField] private Vector3 center;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + center, size);
    }

    public class Baker : Baker<CollisionAABBAuthoring>
    {
        public override void Bake(CollisionAABBAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionAABB
            {
                HalfSize = authoring.size * 0.5f
            });
        }
    }
}
