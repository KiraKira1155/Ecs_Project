using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraObjsct : MonoBehaviour
{
    public static Camera Instance;

    private void Awake()
    {
        Instance = GetComponent<UnityEngine.Camera>();
    }
}
