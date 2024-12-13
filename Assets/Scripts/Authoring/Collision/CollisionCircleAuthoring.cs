using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[System.Serializable]
[BurstCompile]
public struct CollisionCircle : IComponentData
{
    [HideInInspector]
    public CircleShape Circle;
}

[ExecuteAlways]
public class CollisionCircleAuthoring : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Vector2 center;
    [SerializeField] private HitCalculationUtilities.ColliderDirection direction;

    private CircleShape circle;

    private void OnEnable()
    {
        circle = ShapeManager.SetCircle(transform.position, center, radius, direction);
    }

    private void Update()
    {
        DebugDrawUtility.DrawCircle(circle, transform.position);
    }

    public class Baker : Baker<CollisionCircleAuthoring>
    {
        public override void Bake(CollisionCircleAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionCircle
            {
                Circle = ShapeManager.SetCircle(authoring.transform.position, authoring.center, authoring.radius, authoring.direction)
            });
        }
    }
}
