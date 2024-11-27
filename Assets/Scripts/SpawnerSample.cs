using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSample : MonoBehaviour
{
    [SerializeField] private GameObject enemySample;
    void Update()
    {
        var obj = Instantiate(enemySample);
        obj.transform.position = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30));
    }
}
