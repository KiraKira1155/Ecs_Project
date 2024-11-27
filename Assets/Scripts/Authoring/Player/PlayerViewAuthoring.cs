using Unity.Entities;
using UnityEngine;

public class PlayerViewAuthoring : MonoBehaviour
{
    public GameObject Character;
    public class Baker : Baker<PlayerViewAuthoring>
    {
        public override void Bake(PlayerViewAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FirstPersonCharacterView 
            { 
                CharacterEntity = GetEntity(authoring.Character, TransformUsageFlags.Dynamic) 
            });
        }
    }
}
