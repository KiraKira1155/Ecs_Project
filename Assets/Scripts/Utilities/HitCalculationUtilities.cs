using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public struct HitCalculationUtilities
{

    [BurstCompile]
    public bool SphereVSSphere(CollisionSphere attack, CollisionSphere defense, bool checkHitPos, out float3 hitPos)
    {
        var a_r = attack.Radius;
        var d_r = defense.Radius;

        var a_c = attack.CenterPos;
        var d_c = defense.CenterPos;

        var dis_x = d_c.x - a_c.x;
        var dis_y = d_c.y - a_c.y;
        var dis_z = d_c.z - a_c.z;

        var length = dis_x * dis_x + dis_y * dis_y + dis_z * dis_z;

        if (length <= (a_r + d_r) * (a_r + d_r))
        {
            if (!checkHitPos)
            {
                hitPos = float3.zero;
                return true;
            }

            // 中点間ベクトル
            length = math.sqrt(length);

            // 単位化した方向ベクトル
            var normalVec = new float3
            {
                x = dis_x / length,
                y = dis_y / length,
                z = dis_z / length
            };

            // 中点間ベクトル上に境界がある 
            if(length < d_r)
            {
                hitPos.x = a_c.x + a_r * normalVec.x;
                hitPos.y = a_c.y + a_r * normalVec.y;
                hitPos.z = a_c.z + a_r * normalVec.z;
            }
            else
            {
                hitPos.x = d_c.x + d_r * normalVec.x;
                hitPos.y = d_c.y + d_r * normalVec.y;
                hitPos.z = d_c.z + d_r * normalVec.z;
            }

            return true;
        }

        hitPos = float3.zero;
        return false;
    }
}
