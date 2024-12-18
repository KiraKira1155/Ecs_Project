using Unity.Entities;
using UnityEngine;

[System.Serializable]
public struct CharacterView : IComponentData
{
    public Entity CharacterEntity;
}

public class PlayerViewAuthoring : MonoBehaviour
{
    public GameObject Character;
    public class Baker : Baker<PlayerViewAuthoring>
    {
        public override void Bake(PlayerViewAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterView
            { 
                CharacterEntity = GetEntity(authoring.Character, TransformUsageFlags.Dynamic) 
            });
        }
    }
}
