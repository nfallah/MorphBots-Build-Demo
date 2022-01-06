//Uses basic three in addition to System which allows for more fundamental classes and base classes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Placement : MonoBehaviour
{
    // The actual prefab in the game that is placed
    // This is more transparent than a MorphBot to indicate where a MorphBot will place.
    public GameObject morphBotHover;
    // The prefab that the above morphBotHover uses and creates an instance out of
    public GameObject morphBotHoverRef;

    // The actual prefab in the game that is placed
    public GameObject morphBotRef;

    // The parent that all placed MorphBots will attach to
    public GameObject morphBotsPar;

    // The maximum distance of the upcoming Raycasts
    public int maxRaycastDistance;

    // The layers used (both platform and MorphBot)
    public LayerMask gameLayers;

    // The hit information if a Raycast is successfull including the collision point
    RaycastHit raycastHit;

    // Reference to other scripts
    Rulesets rulesets;
    Functions functions;
    Main main;

    // Sets reference scripts and creates a transparent morphBot that will be moved or activated/deactivated depending on mouse position.
    private void Awake()
    {
        rulesets = GetComponent<Rulesets>();
        functions = GetComponent<Functions>();
        main = GetComponent<Main>();
        morphBotHover = Instantiate(morphBotHoverRef, new Vector3(0, 0, 0), Quaternion.identity);
        morphBotHover.name = "MorphBot Highlight";
    }

    // Placement
    private void Update()
    {

        // Does a raycast. If it is touching a platform or morphbot, it will use a function to snap the raycastHit.point to a grid and move the hover MorphBot there.
        // If the raycast does not collide with a platform/MorphBot OR the gridsnapped location is OUTSIDE of the array, the hover MorphBot will disappear.

        Ray ray;

        if (GetComponent<CameraManager>().currentMode == CameraManager.cameraMode.ROTATION)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        else
        {
            ray = new Ray(GetComponent<CameraManager>().playerCamera.transform.position, GetComponent<CameraManager>().playerCamera.transform.forward);
        }

        if (Physics.Raycast(ray, out raycastHit, maxRaycastDistance, gameLayers))
        {
            Vector3 location = functions.Vector3ToGrid(raycastHit);

            if (rulesets.WithinArray(Vector3Int.RoundToInt(location)))
            {
                if (morphBotHover.activeSelf == false)
                {
                    morphBotHover.SetActive(true);
                }

                morphBotHover.transform.position = location;
            }

            else
            {
                if (morphBotHover.activeSelf != false)
                {
                    morphBotHover.SetActive(false);
                }
            }
        }

        else
        {
            if (morphBotHover.activeSelf != false)
            {
                morphBotHover.SetActive(false);
            }
        }

        // If the mouse is touching a platform or MorphBot, a non-hover opaque MorphBot will be placed in the location and named accordingly.
        // The grid at that location will also be updated to unwalkable because there is now a block there. This helps with the pathfinding algorithm.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray2;

            if (GetComponent<CameraManager>().currentMode == CameraManager.cameraMode.ROTATION)
            {
                ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            }

            else
            {
                ray2 = new Ray(GetComponent<CameraManager>().playerCamera.transform.position, GetComponent<CameraManager>().playerCamera.transform.forward);
            }

            if (Physics.Raycast(ray2, out raycastHit, maxRaycastDistance, gameLayers))
            {
                Vector3Int location = functions.Vector3ToGrid(raycastHit);

                if (rulesets.WithinArray(location) && main.grid[location.x, location.y, location.z].walkable)
                {
                    GameObject morphBot = Instantiate(morphBotRef, location, Quaternion.identity);
                    morphBot.transform.SetParent(morphBotsPar.transform);
                    morphBot.name = "MorphBot(" + location.x + ", " + location.y + ", " + location.z + ")";
                    main.grid[location.x, location.y, location.z].walkable = false;

                    if (GetComponent<MaterialManager>().oldMaterial != null && (GetComponent<MaterialManager>().oldMaterial.color == GetComponent<MaterialManager>().currentMaterial.color))
                    {
                        morphBot.GetComponent<MeshRenderer>().material = GetComponent<MaterialManager>().oldMaterial;
                    }

                    else
                    {
                        GetComponent<MaterialManager>().oldMaterial = new Material(GetComponent<MaterialManager>().currentMaterial);
                        GetComponent<MaterialManager>().oldMaterial.color = new Color(GetComponent<MaterialManager>().oldMaterial.color.r, GetComponent<MaterialManager>().oldMaterial.color.g, GetComponent<MaterialManager>().oldMaterial.color.b, 1);
                        morphBot.GetComponent<MeshRenderer>().material = GetComponent<MaterialManager>().oldMaterial;
                    }
                }
            }
        }
    }
}
