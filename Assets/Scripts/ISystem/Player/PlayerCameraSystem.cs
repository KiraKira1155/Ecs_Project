using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[BurstCompile]
partial struct PlayerCameraSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerCamera>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var camera = PlayerCameraObjsct.Instance;
        if (camera != null)
        {
            Entity playerCamera = SystemAPI.GetSingletonEntity<PlayerCamera>();
            LocalToWorld target = SystemAPI.GetComponent<LocalToWorld>(playerCamera);
            camera.transform.SetPositionAndRotation(target.Position, target.Rotation);
        }
    }
}
