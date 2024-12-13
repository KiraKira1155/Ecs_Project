using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct EntityCollision : IComponentData
{
    public Entity Entity;
    public ShapeManager.ShapeType CollisionShapeType;
    public bool IsFreeze;
    private float3 Force;
    [ReadOnly] public float3 CollisionCenterPos;
    [ReadOnly] public float3 MoveSpeed;
    [HideInInspector] public float3 EntityLocalPos;
    [HideInInspector] public float3 PreviousForce;
    [HideInInspector] public bool OnGround;
    [HideInInspector] public bool BeInitSetting;

    [BurstCompile]
    public void AddForce(float3 force)
    {
        Force += force;
    }

    [BurstCompile]
    public float3 GetForce()
    {
        return Force;
    }

    [BurstCompile]
    public void ResetForce()
    {
        Force = 0;
    }
}

class EntityCollisionAuthoring : MonoBehaviour
{
    [SerializeField] private EntityCollision collisionSetting;
    class Baker : Baker<EntityCollisionAuthoring>
    {
        public override void Bake(EntityCollisionAuthoring authoring)
        {
            Entity entity = GetEntity(authoring.collisionSetting.IsFreeze == true ? TransformUsageFlags.None : TransformUsageFlags.Dynamic);
            authoring.collisionSetting.Entity = entity;
            AddComponent(entity, authoring.collisionSetting);

        }
    }
}

