using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct Player : IComponentData
{
    public Entity ControlledCharacterObject;
}

[System.Serializable]
public struct PlayerInputs : IComponentData
{
    public float2 MoveInput;
    public float2 LookInput;
    public FixedInputEvent JumpPressed;
}

[DisallowMultipleComponent]
public class PlayerAuthoring : MonoBehaviour
{
    public GameObject controlledCharacterObject;

    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Player
            {
                ControlledCharacterObject = GetEntity(authoring.controlledCharacterObject, TransformUsageFlags.Dynamic),
            });
            AddComponent<PlayerInputs>(entity);
        }
    }
}
