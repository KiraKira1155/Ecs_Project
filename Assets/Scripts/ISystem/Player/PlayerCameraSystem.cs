using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(UserMoveCameraViewSystem))]
partial struct PlayerCameraSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerCamera>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var camera = PlayerCameraObject.Instance;
        if (camera != null)
        {
            Entity playerCamera = SystemAPI.GetSingletonEntity<PlayerCamera>();
            LocalToWorld target = SystemAPI.GetComponent<LocalToWorld>(playerCamera);
            camera.transform.SetPositionAndRotation(target.Position + new float3(0, 0.5f, -5.0f), target.Rotation);
            Vector3.Lerp();
    }
}
