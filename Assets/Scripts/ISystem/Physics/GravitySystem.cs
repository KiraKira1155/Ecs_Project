using System.Threading;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(EntityCollisionHitSystem))]
partial struct GravitySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var time = SystemAPI.Time.DeltaTime;

        new GravityJob
        {
            deltaTime = time,
        }.ScheduleParallel();
    }
}

[BurstCompile] 
partial struct GravityJob : IJobEntity
{
    public float deltaTime;

    [BurstCompile]
    public void Execute(ref GravityEffect gravity, ref EntityCollision collision)
    {
        if (!collision.BeInitSetting)
            return;

        if (!collision.OnGround && !gravity.IsUnderwater)
        {
            gravity.FallTime += deltaTime;
            collision.AddForce(FreeFallingSpeed(gravity.Mass, gravity.ResistanceAir, gravity.FallTime, collision.PreviousForce.y, gravity.TerminalVelocity));
        }
        else if (collision.OnGround)
        {
            gravity.FallTime = 0;
        }
    }

    /// <summary>
    /// 自由落下速度計算
    /// </summary>
    /// <param name="m"></param>
    /// <param name="k"></param>
    /// <param name="t"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    [BurstCompile]
    public float3 FreeFallingSpeed(float m, float k, float t, float v, float terminalV)
    {
        if (k == 0 || v == 0)
            return FreeFallingSpeed(t);

        m = m == 0 ? 0.000001f : m;

        var F = k * v * v;

        //var velocity = m * PhysicsConstantUtility.G / F * (1 - math.pow(PhysicsConstantUtility.E, (-F / m * t)));

        //return new float3(0, -velocity, 0);

        if(v <= -terminalV)
        {
            return new float3(0, -terminalV, 0);
        }

        //ルンゲ＝クッタ法
        var h = t / 2;
        var k1 = EquationOfMotion(v, F, m);
        var k2 = EquationOfMotion(v + h * k1, F, m);
        var k3 = EquationOfMotion(v + h * k2, F, m);
        var k4 = EquationOfMotion(v + t * k3, F, m);

        return new float3(0, (k1 + 2 * k2 + 2 * k3 + k4) * t / 6, 0);
    }

    [BurstCompile]
    public float3 FreeFallingSpeed(float t)
    {
        return new float3(0, (-PhysicsConstantUtility.G * t), 0);
    }

    [BurstCompile]
    private float EquationOfMotion(float v, float k, float m)
    {
        return -PhysicsConstantUtility.G -(k * v) / m;
    }
}

[BurstCompile]
partial struct BuoyancyJob : IJobEntity
{
    private float deltaTime;

    [BurstCompile]
    public void Execute(ref GravityEffect gravity, ref EntityCollision collision)
    {
        if (!collision.OnGround && gravity.IsUnderwater)
        {
            collision.AddForce(Buoyancy());
        }
    }

    public float3 Buoyancy()

    {

        return new float3();
    }
}

