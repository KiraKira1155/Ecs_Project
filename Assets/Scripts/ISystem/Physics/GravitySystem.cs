using System.Threading;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(EntityCollisionSettingSystem))]
partial struct GravitySystem : ISystem
{
    private float TimeCount;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        TimeCount += SystemAPI.Time.DeltaTime;

        if (TimeCount >= 5)
            return;

        new GravityJob
        {
            deltaTime = TimeCount,
        }.ScheduleParallel();
    }
}

[BurstCompile] 
partial struct GravityJob : IJobEntity
{
    public float deltaTime;

    private const float G = -9.80665f;
    private const float Air = 0.0000182f;
    private const float Water = 0.001f;

    [BurstCompile]
    public void Execute(ref GravityEffect gravity, ref EntityCollision collision)
    {
        if (!collision.OnGround)
        {
            collision.AddForce(FreeFallingSpeed(gravity.Mass, gravity.AirResistanceCoefficient, deltaTime, collision.PreviousForce.y));
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
    public float3 FreeFallingSpeed(float m, float k, float t, float v)
    {
        m = m == 0 ? 0.000001f : m;

        //慣性抵抗
        k = 0.5f * k * 0.5f *  Air * v * v; // 0.5は断面積、0.5は公式の1/2

        if (k == 0)
            return FreeFallingSpeed(t);
        return new float3(0, ((m * G) / k * (1 - math.pow(math.E, -((k / m) * t)))), 0);

        ////ルンゲ＝クッタ法
        //var h = t / 2;
        //var k1 = EquationOfMotion(v, k, m);
        //var k2 = EquationOfMotion(v + h * k1, k, m);
        //var k3 = EquationOfMotion(v + h * k2, k, m);
        //var k4 = EquationOfMotion(v + t * k3, k, m);

        //return new float3(0, (k1 + 2 * k2 + 2 * k3 + k4) * t / 6, 0);
    }

    [BurstCompile]
    public float3 FreeFallingSpeed(float t)
    {
        return new float3(0, (G * t), 0);
    }

    [BurstCompile]
    private float EquationOfMotion(float v, float k, float m)
    {
        return G-(k * v) / m;
    }
}
