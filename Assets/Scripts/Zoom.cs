//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    // Script used to move the camera's local position depending on whether it was scrolling in or out.


    // Amplifies how much you can zoom in/out
    public float zoomSpeed;

    // The maximum value the camera can be zoomed out at
    public float maxZoom;

    // The current Zoom of the camera.
    [HideInInspector] public Vector3 currentZoom;

    // Sets currentZoom
    private void Awake()
    {
        if (FindObjectOfType<ClearChecker>() != null)
        {
            currentZoom = FindObjectOfType<ClearChecker>().cameraZoom;
            this.transform.localPosition = currentZoom;
        }

        else
        {
            currentZoom = this.transform.localPosition;
        }
    }

    // Updates and clamps currentZoom based on how much (if at all) the user has scrolled.
    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            currentZoom.z = Mathf.Clamp(currentZoom.z + (Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed), maxZoom * -1, -3);
            this.transform.localPosition = currentZoom;
        }
    }
}
