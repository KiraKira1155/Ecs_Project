using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public struct HitCalculationUtilities
{
    public enum ColliderDirection
    {
        XY,
        XZ,
        YZ
    }

    [BurstCompile]
    public bool AABBVSPillar(in AABBShape attack, in PillarShape defense, in bool checkHitPos, out float3 hitPos)
    {
        hitPos = float3.zero;
        return false;
    }

    [BurstCompile]
    public bool AABBVSSphere(in AABBShape attack, in SphereShape defense, in bool checkHitPos, out float3 hitPos)
    {
        var length = 0.0f;
        var distance = 0.0f;

        // x, y, z の距離の合計が最短距離

        // x
        if(defense.EntityCenterPos.x < attack.MinPos.x)
        {
            distance = attack.MinPos.x - defense.EntityCenterPos.x;
        }
        else if (defense.EntityCenterPos.x > attack.MaxPos.x)
        {
            distance = attack.MaxPos.x - defense.EntityCenterPos.x;
        }

        length += distance * distance;
        hitPos.x = distance;
        distance = 0.0f;

        // y
        if (defense.EntityCenterPos.y < attack.MinPos.y)
        {
            distance = attack.MinPos.y - defense.EntityCenterPos.y;
        }
        else if (defense.EntityCenterPos.y > attack.MaxPos.y)
        {
            distance = attack.MaxPos.y - defense.EntityCenterPos.y;
        }

        length += distance * distance;
        hitPos.y = distance;
        distance = 0.0f;

        // z
        if (defense.EntityCenterPos.z < attack.MinPos.z)
        {
            distance = attack.MinPos.z - defense.EntityCenterPos.z;
        }
        else if (defense.EntityCenterPos.z > attack.MaxPos.z)
        {
            distance = attack.MaxPos.z - defense.EntityCenterPos.z;
        }

        length += distance * distance;
        hitPos.z = distance;

        if(length > defense.DoubleRadius)
        {
            hitPos = float3.zero;
            return false;
        }

        if (checkHitPos)
        {
            hitPos.x = defense.EntityCenterPos.x + hitPos.x;
            hitPos.y = defense.EntityCenterPos.y + hitPos.y;
            hitPos.y = defense.EntityCenterPos.y + hitPos.y;
        }
        else
        {
            return true;
        }
        return true;
    }

    [BurstCompile]
    public bool AABBVSAABB(in AABBShape attack, in AABBShape defense, in bool checkHitPos, out float3 hitPos)
    {
        if(attack.MaxPos.x < defense.MinPos.x || attack.MaxPos.y < defense.MinPos.y || attack.MaxPos.z < defense.MinPos.z ||
            defense.MaxPos.x < attack.MinPos.x || defense.MaxPos.y < attack.MinPos.y || defense.MaxPos.z < attack.MinPos.z)
        {
            hitPos = float3.zero;
            return false;
        }

        if (!checkHitPos)
        {
            hitPos = float3.zero;
            return true;
        }

        // xチェック
        if(attack.EntityCenterPos.x < defense.MinPos.x)
        {
            hitPos.x = defense.MinPos.x;
        }
        else if (attack.EntityCenterPos.x > defense.MaxPos.x)
        {
            hitPos.x = defense.MaxPos.x;
        }
        else
        {
            hitPos.x = attack.EntityCenterPos.x;
        }

        // yチェック
        if (attack.EntityCenterPos.y < defense.MinPos.y)
        {
            hitPos.y = defense.MinPos.y;
        }
        else if (attack.EntityCenterPos.y > defense.MaxPos.y)
        {
            hitPos.y = defense.MaxPos.y;
        }
        else
        {
            hitPos.y = attack.EntityCenterPos.y;
        }

        // zチェック
        if (attack.EntityCenterPos.z < defense.MinPos.z)
        {
            hitPos.z = defense.MinPos.z;
        }
        else if (attack.EntityCenterPos.z > defense.MaxPos.z)
        {
            hitPos.z = defense.MaxPos.z;
        }
        else
        {
            hitPos.z = attack.EntityCenterPos.z;
        }

        return true;
    }

    [BurstCompile]
    public bool SphereVSSphere(in SphereShape attack, in SphereShape defense, in bool checkHitPos, out float3 hitPos)
    {
        var dis_x = defense.EntityCenterPos.x - attack.EntityCenterPos.x;
        var dis_y = defense.EntityCenterPos.y - attack.EntityCenterPos.y;
        var dis_z = defense.EntityCenterPos.z - attack.EntityCenterPos.z;

        var length = dis_x * dis_x + dis_y * dis_y + dis_z * dis_z;

        if (length <= (attack.Radius + defense.Radius) * (attack.Radius + defense.Radius))
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
            if(length < defense.Radius)
            {
                hitPos.x = attack.EntityCenterPos.x + attack.Radius * normalVec.x;
                hitPos.y = attack.EntityCenterPos.y + attack.Radius * normalVec.y;
                hitPos.z = attack.EntityCenterPos.z + attack.Radius * normalVec.z;
            }
            else
            {
                hitPos.x = defense.EntityCenterPos.x + defense.Radius * normalVec.x;
                hitPos.y = defense.EntityCenterPos.y + defense.Radius * normalVec.y;
                hitPos.z = defense.EntityCenterPos.z + defense.Radius * normalVec.z;
            }

            return true;
        }

        hitPos = float3.zero;
        return false;
    }

    [BurstCompile]
    public bool SphereVSPillar(in SphereShape attack, in PillarShape defense, in bool checkHitPos, out float3 hitPos)
    {
        var circlePos = float3.zero;
        var attackCircle = ShapeManager.SetCircle(attack.EntityCenterPos, new float2(attack.ShapeCenterPos.x, attack.ShapeCenterPos.z), attack.Radius, ColliderDirection.XZ);
        
        // xz平面　円判定
        if (!CircleVSCircle(attackCircle, defense.Circle, checkHitPos, out circlePos))
        {
            hitPos = float3.zero;
            return false;
        }

        // xy, yz 平面　円vs矩形
        // xyから
        attackCircle = ShapeManager.SetCircle(attack.EntityCenterPos, new float2(attack.ShapeCenterPos.x, attack.ShapeCenterPos.y), attack.Radius, ColliderDirection.XY);

        var defenseQuad = ShapeManager.SetQuad
            (defense.EntityCenterPos, new float2(defense.ShapeCenterPos.x, defense.ShapeCenterPos.y),
            new float2(defense.Circle.Radius * 2.0f, defense.Height), ColliderDirection.XY);

        var quadPos = float3.zero;
        // xyの接触判定
        if(!QuadVSCircle(defenseQuad, attackCircle, checkHitPos, out quadPos))
        {
            hitPos = float3.zero;
            return false;
        }

        // xyがあたっていたのでyz判定
        attackCircle = ShapeManager.SetCircle(attack.EntityCenterPos, new float2(attack.ShapeCenterPos.y, attack.ShapeCenterPos.z), attack.Radius, ColliderDirection.YZ);
        defenseQuad = ShapeManager.SetQuad
            (defense.EntityCenterPos, new float2(defense.ShapeCenterPos.y, defense.ShapeCenterPos.z),
            new float2(defense.Circle.Radius * 2.0f, defense.Height), ColliderDirection.YZ);

        if (!QuadVSCircle(defenseQuad, attackCircle, checkHitPos, out quadPos))
        {
            hitPos = float3.zero;
            return false;
        }


        hitPos = circlePos;
        hitPos.y = quadPos.y;

        return true;
    }

    [BurstCompile]
    public bool CircleVSCircle(in CircleShape attack, in CircleShape defense, in bool checkHitPos, out float3 hitPos)
    {
        var dis_x = defense.EntityCenterPos.x - attack.EntityCenterPos.x;
        var dis_y = defense.EntityCenterPos.y - attack.EntityCenterPos.y;

        var length = dis_x * dis_x + dis_y * dis_y;
        if(length <= (attack.Radius + defense.Radius) * (attack.Radius + defense.Radius))
        {
            if (!checkHitPos)
            {
                hitPos = float3.zero;
                return true;
            }

            length = math.sqrt(length);
            var normalVec = new float2
            {
                x = dis_x / length,
                y = dis_y / length,
            };

            var pos = float2.zero;

            if(length < attack.Radius)
            {
                pos.x = attack.EntityCenterPos.x + attack.Radius * normalVec.x;
                pos.y = attack.EntityCenterPos.y + attack.Radius * normalVec.y;
            }
            else
            {
                pos.x = defense.EntityCenterPos.x + defense.Radius * normalVec.x;
                pos.y = defense.EntityCenterPos.y + defense.Radius * normalVec.y;
            }

            switch (attack.CollisionDirection)
            {
                case ColliderDirection.XY:
                    hitPos.x = pos.x;
                    hitPos.y = pos.y;
                    hitPos.z = 0;
                    break;

                case ColliderDirection.XZ:
                    hitPos.x = pos.x;
                    hitPos.y = 0;
                    hitPos.z = pos.y;
                    break;

                default:
                    hitPos.x = 0;
                    hitPos.y = pos.x;
                    hitPos.z = pos.y;
                    break;
            }

            return true;
        }

        hitPos = float3.zero;
        return false;
    }

    [BurstCompile]
    public bool QuadVSCircle(in QuadShape attack, in CircleShape defense, in bool checkHitPos, out float3 hitPos)
    {
        // 円の中心から矩形の最も近い外周との距離を求めて半径以内なら当たっている
        var dis_x = math.max(0.0f, math.abs(defense.EntityCenterPos.x - attack.EntityCenterPos.x) - attack.HalfSize.x);
        var dis_y = math.max(0.0f, math.abs(defense.EntityCenterPos.y - attack.EntityCenterPos.y) - attack.HalfSize.y);
        var length = dis_x * dis_x + dis_y * dis_y;

        if (length <= defense.DoubleRadius)
        {
            if (!checkHitPos)
            {
                hitPos = float3.zero;
                return true;
            }

            length = math.sqrt(length);

            var normalVec = new float2
            {
                x = (defense.EntityCenterPos.x - attack.EntityCenterPos.x) / length,
                y = (defense.EntityCenterPos.y - attack.EntityCenterPos.y) / length,
            };

            var pos = float2.zero;

            if (length < defense.Radius)
            {
                pos.x = defense.EntityCenterPos.x + defense.Radius * normalVec.x;
                pos.y = defense.EntityCenterPos.y + defense.Radius * normalVec.y;
            }
            else
            {
                pos.x = attack.EntityCenterPos.x + attack.HalfSize.x * normalVec.x;
                pos.y = attack.EntityCenterPos.y + attack.HalfSize.y * normalVec.y;
            }

            switch (attack.CollisionDirection)
            {
                case ColliderDirection.XY:
                    hitPos.x = pos.x;
                    hitPos.y = pos.y;
                    hitPos.z = 0;
                    break;

                case ColliderDirection.XZ:
                    hitPos.x = pos.x;
                    hitPos.y = 0;
                    hitPos.z = pos.y;
                    break;

                default:
                    hitPos.x = 0;
                    hitPos.y = pos.x;
                    hitPos.z = pos.y;
                    break;
            }
            return true;
        }

        hitPos = float3.zero;
        return false;
    }
}
