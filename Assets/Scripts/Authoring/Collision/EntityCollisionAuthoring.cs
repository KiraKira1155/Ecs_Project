using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct EntityCollision : IComponentData
{
    [ReadOnly] public Entity Entity;
    [ReadOnly] public int ID;

    [ReadOnly] public int ObjectID;
    [HideInInspector]public bool HasParentEntity;

    [ReadOnly] public bool IsFreeze;

    [ReadOnly] public float3 MoveSpeed;

    [ReadOnly] public float3 EntityLocalPos;
    [ReadOnly] public bool OnGround;
    [HideInInspector] public bool BeInitSetting;

    [HideInInspector] public bool CanCheckHitPos;

    [HideInInspector] public float3 PreviousForce;
    private float3 Force;

    [HideInInspector] public int StateFlag;

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

public struct HitContainerBuffer
{
    public List<Entity> HitEntity;
    public List<float3> HitPos;
}

class EntityCollisionAuthoring : MonoBehaviour
{
    [Header("�����Ȃ��I�u�W�F�N�g�̏ꍇ��True")]
    [SerializeField] private bool isFrees;

    [Header("�����蔻��Ώۂ̍ŏ�ʐe�I�u�W�F�N�g�o�^\n" +
        "�ݒ肪�Ȃ���΂��̃R���|�[�l���g�����Ă���I�u�W�F�N�g���ݒ肳���")]
    [SerializeField] private GameObject collisionParentObj;
    public class Baker : Baker<EntityCollisionAuthoring>
    {
        public override void Bake(EntityCollisionAuthoring authoring)
        {
            var entity = Entity.Null;
            var id = 0;
            if (authoring.collisionParentObj == null)
            {
                entity = GetEntity(authoring.isFrees == true ? TransformUsageFlags.None : TransformUsageFlags.Dynamic);
                id = authoring.gameObject.GetInstanceID();
            }
            else
            {
                entity = GetEntity(authoring.collisionParentObj.GetComponent<EntityCollisionAuthoring>().isFrees == true ? TransformUsageFlags.None : TransformUsageFlags.Dynamic);
                id = authoring.collisionParentObj.GetInstanceID();
            }
            AddComponent(entity, new EntityCollision
            {
                Entity = entity,
                ObjectID = id,
                HasParentEntity = authoring.collisionParentObj == null ? false : true
            });
        }
    }
}

