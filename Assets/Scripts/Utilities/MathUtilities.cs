using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public static class MathUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 Lerp(float3 start, float3 end, float currentPos)
    {
        currentPos = Clamp01(currentPos);
        return new float3(start.x + (end.x - start.x) * currentPos, start.y + (end.y - start.y) * currentPos, start.z + (end.z - start.z) * currentPos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp01(float value)
    {
        if (value < 0f)
        {
            return 0f;
        }

        if (value > 1f)
        {
            return 1f;
        }

        return value;
    }
}
