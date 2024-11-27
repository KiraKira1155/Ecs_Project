using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct EntityCollision : IComponentData
{
    public bool isFreeze;
    [HideInInspector] public bool OnGround;
    [HideInInspector] public float3 CenterPos;
}

class EntityCollisionAuthoring : MonoBehaviour
{
    [SerializeField] private EntityCollision collisionSetting;
    class Baker : Baker<EntityCollisionAuthoring>
    {
        public override void Bake(EntityCollisionAuthoring authoring)
        {
            authoring.collisionSetting.CenterPos = authoring.transform.position;
            Entity entity = GetEntity(authoring.collisionSetting.isFreeze == true ? TransformUsageFlags.None : TransformUsageFlags.Dynamic);


        }
    }
}

