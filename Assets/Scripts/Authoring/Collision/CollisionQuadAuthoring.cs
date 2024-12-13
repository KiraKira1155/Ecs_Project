using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct CollisionQuad : IComponentData
{
    [HideInInspector]
    public QuadShape Quad;
}

[ExecuteAlways]
public class CollisionQuadAuthoring : MonoBehaviour
{
    [SerializeField] private Vector2 center;
    [SerializeField] private Vector2 size;
    [SerializeField] private HitCalculationUtilities.ColliderDirection direction;

    private QuadShape quad;

    private void OnEnable()
    {
        quad = ShapeManager.SetQuad(transform.position, center, size, direction);
    }

    public class Baker : Baker<CollisionQuadAuthoring>
    {
        public override void Bake(CollisionQuadAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionQuad
            {
                Quad = ShapeManager.SetQuad(authoring.transform.position, authoring.center, authoring.size, authoring.direction)
            });
        }
    }
}
