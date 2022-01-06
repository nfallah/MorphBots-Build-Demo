using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderReset : MonoBehaviour
{
    public float blockSpeed, sensitivity, movementSpeed;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
