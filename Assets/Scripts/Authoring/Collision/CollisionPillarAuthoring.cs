using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionPillar : IComponentData
{
    [HideInInspector]
    public PillarShape Pillar;

    [ReadOnly]
    public uint ID;
    public Entity Entity;
    [HideInInspector]
    public bool BeInitSetting;
}

[ExecuteAlways]
public class CollisionPillarAuthoring : MonoBehaviour
{
    [Header("�����蔻��͈͂̊�{�ݒ�")]
    [SerializeField] private Vector3 center;
    [SerializeField] private float height;
    [SerializeField] private float radius;

    [Header("�����蔻��Ώۂ̍ŏ�ʐe�I�u�W�F�N�g�o�^")]
    [SerializeField] private GameObject collisionParentObj;

    private PillarShape pillar;

    private void OnEnable()
    {
        pillar = ShapeManager.SetPillar(transform.position, center, radius, height);
    }

    private void Update()
    {
        DebugDrawUtility.DrawPillar(pillar, transform.position);
    }

    public class Baker : Baker<CollisionPillarAuthoring>
    {
        public override void Bake(CollisionPillarAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            Entity targetEntity = GetEntity(authoring.collisionParentObj, TransformUsageFlags.Dynamic);
            if (authoring.collisionParentObj == null)
            {
                targetEntity = entity;
            }

            AddComponent(entity, new CollisionPillar
            {
                Pillar = ShapeManager.SetPillar(authoring.transform.position, authoring.center, authoring.radius, authoring.height),
                Entity = targetEntity
            });
        }
    }
}
