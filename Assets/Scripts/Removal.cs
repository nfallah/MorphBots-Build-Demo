//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Removal : MonoBehaviour
{
    // the current MorphBot that can be deleted
    GameObject currentMorphBot;

    // The game layers (platform, MorphBot)
    public LayerMask gameLayers;

    // Results of Raycast if it was successful
    RaycastHit raycastHit;

    // Raycast max distance
    public int maxRaycastDistance;

    // Reference to main script
    Main main;

    // Sets the main script during runtime so that it can be used with this script.
    private void Awake()
    {
        main = GetComponent<Main>();
    }

    private void Update()
    {
        Ray ray;

        if (GetComponent<CameraManager>().currentMode == CameraManager.cameraMode.ROTATION)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        else
        {
            ray = new Ray(GetComponent<CameraManager>().playerCamera.transform.position, GetComponent<CameraManager>().playerCamera.transform.forward);
        }

        // Checks every frame to see if camera is pointing to any cube, registering it as currentMorphBot if true
        if (Physics.Raycast(ray, out raycastHit, maxRaycastDistance, gameLayers))
        {
            if (raycastHit.transform.gameObject.layer == 9)
            {
                currentMorphBot = raycastHit.transform.gameObject;
            }

            // Sets currentMorphBot to null if no cube is detected (meaning the mouse directly sees a platform instead, which cannot be deleted).
            else
            {
                currentMorphBot = null;
            }
        }

        // Sets currentMorphBot to null because no platform OR MorphBot was detected.
        else
        {
            currentMorphBot = null;
        }

        // Deletes currentMorphBot if it is not null
        if (Input.GetMouseButtonDown(1))
        {
            if (currentMorphBot != null)
            {
                Vector3Int location = Vector3Int.RoundToInt(currentMorphBot.transform.position);
                main.grid[location.x, location.y, location.z].walkable = true;
                Destroy(currentMorphBot);
            }
        }
    }
}
