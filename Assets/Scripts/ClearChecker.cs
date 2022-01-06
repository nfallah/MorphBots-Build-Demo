using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearChecker : MonoBehaviour
{
    public int[] ints;
    public Vector3 cameraRotation;
    public Vector3 cameraZoom, flightPos, flightRot, rotationRot;

    public CameraManager.cameraMode currentMode;



    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
