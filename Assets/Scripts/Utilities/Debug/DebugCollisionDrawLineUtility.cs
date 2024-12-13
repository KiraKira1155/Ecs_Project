using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct DebugCollisionDrawLineUtility
{
    [BurstCompile]
    public void DrawAABB(Vector3 position, Vector3 size, Color lineColor)
    {

    }

    [BurstCompile]
    private struct Box
    {
        public Vector2 point;
        public Vector2 size;

        public readonly Vector2 RightUp()
        {
            return new Vector2(point.x + size.x / 2, point.y + size.y / 2);
        }

        public readonly Vector2 RightDown()
        {
            return new Vector2(point.x + size.x / 2, point.y - size.y / 2);
        }

        public readonly Vector2 LeftUp()
        {
            return new Vector2(point.x - size.x / 2, point.y + size.y / 2);
        }

        public readonly Vector2 LeftDown()
        {
            return new Vector2(point.x - size.x / 2, point.y - size.y / 2);
        }
    }
}