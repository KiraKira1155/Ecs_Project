using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class EnemySample : MonoBehaviour
{
    private float deltaTime;
    void Start()
    {

    }
    void Update()
    {
        deltaTime = Time.deltaTime;
        transform.rotation = math.mul(quaternion.AxisAngle(new float3(15, 30, 45), 0.05f * deltaTime), math.normalize(transform.rotation));
    }
}
