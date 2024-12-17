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
}

[ExecuteAlways]
public class CollisionPillarAuthoring : MonoBehaviour
{
    [Header("ìñÇΩÇËîªíËîÕàÕÇÃäÓñ{ê›íË")]
    [SerializeField] private Vector3 center;
    [SerializeField] private float height;
    [SerializeField] private float radius;

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

            AddComponent(entity, new CollisionPillar
            {
                Pillar = ShapeManager.SetPillar(authoring.transform.position, authoring.center, authoring.radius, authoring.height)
            });
        }
    }
}
