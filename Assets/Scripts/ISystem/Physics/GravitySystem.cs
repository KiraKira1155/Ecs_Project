using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
partial struct GravitySystem : ISystem
{
    private const float G = 9.80665f;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GravityEffect>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (gravity, transForm, collision) in SystemAPI.Query<RefRW<GravityEffect>, RefRW<LocalTransform>, RefRO<EntityCollision>>())
        {
            switch (collision.ValueRO.OnGround)
            {
                case true:
                    gravity.ValueRW.FallTime = 0.0f;
                    gravity.ValueRW.CurrentSpeed = 0.0f;
                    break;


                case false:
                    var deltaTime = SystemAPI.Time.DeltaTime;

                    gravity.ValueRW.CurrentSpeed += FreeFallingSpeed(gravity.ValueRO.Mass, gravity.ValueRO.AirResistanceCoefficient, deltaTime, gravity.ValueRO.CurrentSpeed);

                    transForm.ValueRW.Position.y += gravity.ValueRO.CurrentSpeed * deltaTime;
                    gravity.ValueRW.FallDistance += -gravity.ValueRO.CurrentSpeed * deltaTime;

                    gravity.ValueRW.FallTime += deltaTime;
                    break;
            }
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
    public float FreeFallingSpeed(float m, float k, float t, float v)
    {
        m = m == 0 ? 0.000001f : m * 1000;
        k = k == 0 ? 0 : k * 1000;
        var h = t / 2;

        //ルンゲ＝クッタ法
        var k1 = EquationOfMotion(v, k, m);
        var k2 = EquationOfMotion(v + h * k1, k, m);
        var k3 = EquationOfMotion(v + h * k2, k, m);
        var k4 = EquationOfMotion(v + t * k3, k, m);

        return (k1 + 2 * k2 + 2 * k3 + k4) * t / 6;
    }

    [BurstCompile]
    private float EquationOfMotion(float x, float k, float m)
    {
        return -G - (k * x / m);
    }
}
