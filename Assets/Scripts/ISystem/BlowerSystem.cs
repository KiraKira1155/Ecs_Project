using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;

[BurstCompile]
[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
partial struct BlowerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Blower>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new BlowerJob
        {
            BlowerGroup = SystemAPI.GetComponentLookup<Blower>(),
            MassGroup = SystemAPI.GetComponentLookup<PhysicsMass>(),
            VelocityGroup = SystemAPI.GetComponentLookup<PhysicsVelocity>(),
            DeltaTime = SystemAPI.Time.DeltaTime
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }
}

[BurstCompile]
partial struct BlowerJob : ITriggerEventsJob
{
    [ReadOnly] public ComponentLookup<Blower> BlowerGroup;
    [ReadOnly] public ComponentLookup<PhysicsMass> MassGroup;
    public ComponentLookup<PhysicsVelocity> VelocityGroup;
    public float DeltaTime;

    [BurstCompile]
    public void Execute(TriggerEvent ev)
    {
        var aIsBlower = BlowerGroup.HasComponent(ev.EntityA);
        var bIsBlower = BlowerGroup.HasComponent(ev.EntityB);

        var aIsObject = VelocityGroup.HasComponent(ev.EntityA);
        var bIsObject = VelocityGroup.HasComponent(ev.EntityB);

        // We only process blower/object case.
        // (Reject blower/blower and object/object cases.)
        if (!(aIsBlower ^ bIsBlower)) return;
        if (!(aIsObject ^ bIsObject)) return;

        var (blowerEntity, objectEntity) =
          aIsBlower ? (ev.EntityA, ev.EntityB) : (ev.EntityB, ev.EntityA);

        var blower = BlowerGroup[blowerEntity];
        var mass = MassGroup[objectEntity];
        var velocity = VelocityGroup.GetRefRW(objectEntity);

        velocity.ValueRW.ApplyLinearImpulse(mass, blower.Force * DeltaTime);
    }
}

