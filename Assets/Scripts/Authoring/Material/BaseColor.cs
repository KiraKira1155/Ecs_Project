using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BaseColor : MonoBehaviour
{
    [SerializeField] private float4 setColor;
    protected float3 color { get { return new float3(setColor.x, setColor.y, setColor.z); } }
    protected float alpha { get { return setColor.w; } }
}
