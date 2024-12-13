using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionAABB : IComponentData
{
    [HideInInspector]
    public AABBShape AABB;

    [ReadOnly]
    public uint ID;
    public Entity Entity;
    [HideInInspector]
    public bool BeInitSetting;
}

[ExecuteAlways]
public class CollisionAABBAuthoring : MonoBehaviour
{
    [Header("�����蔻��͈͂̊�{�ݒ�")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;

    [Header("�����蔻��Ώۂ̍ŏ�ʐe�I�u�W�F�N�g�o�^")]
    [SerializeField] private GameObject collisionParentObj;

    private AABBShape aabb;

    private void OnEnable()
    {
        aabb = ShapeManager.SetAABB(transform.position, center, size);
    }

    private void Update()
    {
        DebugDrawUtility.DrawAABB(aabb, transform.position + center);
    }

    public class Baker : Baker<CollisionAABBAuthoring>
    {
        public override void Bake(CollisionAABBAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new CollisionAABB
            {
                AABB = ShapeManager.SetAABB(authoring.transform.position, authoring.center, authoring.size)
            });
        }
    }
}
