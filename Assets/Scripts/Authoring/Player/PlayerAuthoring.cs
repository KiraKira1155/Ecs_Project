using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
[BurstCompile]
public struct Player : IComponentData
{
    [HideInInspector]
    public Entity PlayerMeshObject;
}

[System.Serializable]
[BurstCompile]
public struct PlayerInputs : IComponentData
{
    public float2 MoveInput;
    public float2 LookInput;
    public FixedInputEvent JumpPressed;
}

[DisallowMultipleComponent]
public class PlayerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject playerMeshObject;
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Player
            {
                PlayerMeshObject = GetEntity(authoring.playerMeshObject, TransformUsageFlags.None),
            });
            AddComponent<PlayerInputs>(entity);
        }
    }
}
