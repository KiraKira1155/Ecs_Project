using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.CharacterController;
using Unity.Mathematics;

public static class PlayerControlUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StandardGroundMove_Interpolated(ref float3 velocity, float3 targetVelocity, float sharpness, float deltaTime, float3 groundingUp, float3 groundedHitNormal)
    {
        velocity = MathUtilities.ReorientVectorOnPlaneAlongDirection(velocity, groundedHitNormal, groundingUp);
        targetVelocity = MathUtilities.ReorientVectorOnPlaneAlongDirection(targetVelocity, groundedHitNormal, groundingUp);
        InterpolateVelocityTowardsTarget(ref velocity, targetVelocity, deltaTime, sharpness);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InterpolateVelocityTowardsTarget(ref float3 velocity, float3 targetVelocity, float deltaTime, float interpolationSharpness)
    {
        velocity = math.lerp(velocity, targetVelocity, MathUtilities.GetSharpnessInterpolant(interpolationSharpness, deltaTime));
    }


    /// <summary>
    /// ÉWÉÉÉìÉvèàóù
    /// </summary>
    /// <param name="characterBody"></param>
    /// <param name="jumpVelocity"></param>
    /// <param name="cancelVelocityBeforeJump"></param>
    /// <param name="velocityCancelingUpDirection"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void StandardJump(ref KinematicCharacterBody characterBody, float3 jumpVelocity, bool cancelVelocityBeforeJump, float3 velocityCancelingUpDirection)
    {
        characterBody.IsGrounded = false;
        characterBody.GroundHit = default;

        if (cancelVelocityBeforeJump)
        {
            characterBody.RelativeVelocity = MathUtilities.ProjectOnPlane(characterBody.RelativeVelocity, velocityCancelingUpDirection);
        }

        characterBody.RelativeVelocity += jumpVelocity;
    }
}
