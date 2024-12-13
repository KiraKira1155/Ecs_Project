using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionSphere : IComponentData
{
    [HideInInspector]
    public SphereShape Sphere;

    [ReadOnly]
    public uint ID;
    public Entity Entity;
    [HideInInspector]
    public bool BeInitSetting;
}

[ExecuteAlways]
public class CollisionSphereAuthoring : MonoBehaviour
{
    [Header("当たり判定範囲の基本設定")]
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius;

    [Header("当たり判定対象の最上位親オブジェクト登録")]
    [SerializeField] private GameObject collisionParentObj;

    private SphereShape sphere;

    private void OnEnable()
    {
        sphere = ShapeManager.SetSphere(transform.position, center, radius);
    }

    private void Update()
    {
        DebugDrawUtility.DrawSphere(sphere, transform.position + center);
    }

    public class Baker : Baker<CollisionSphereAuthoring>
    {
        public override void Bake(CollisionSphereAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new CollisionSphere
            {
                Sphere = ShapeManager.SetSphere(authoring.transform.position, authoring.center, authoring.radius)
            });
        }
    }
}
