//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Script used to "select" MorphBots. If a MorphBot is selected, then pathfinding can be intiated to where the mouse is pointing.
    // Only usable in Movement mode

    // The layers of the Platform and MorphBot prefabs. Used in a Raycast to find blocks to select. 
    //  If there is a Platform block on top of a MorphBot, that MorphBot cannot be selected if the mouse is pointing down at the Platform block (the ray does not go through).
    public LayerMask gameLayers;

    // The max distance of the said raycast.
    public float maxDistance;

    // The result of the raycast (if it even happens) that stores many different things like the point of collision.
    RaycastHit raycastHit;

    // The currentMorphBot is the MorphBot that is selected at the moment. If it is NOT null, then pathfinding can be initiated.
    public GameObject currentMorphBot;

    // A reference to the "Selection" script.
    Selection selection;

    // The key used to initiate pathfinding.
    public KeyCode translate;

    // References to the "Rulesets", "Functions", "Main", and "Pathfinding" scripts.
    Rulesets rulesets;
    Functions functions;
    Pathfinding pathfinding;
    Main main;

    // Is true if a MorphBot is already moving due to pathfinding. This disabled many things like the switching of modes or the selection of other MorphBots until it is false.
    public bool isPathfinding;

    // Sets all the script reference variables for later use.
    private void Awake()
    {
        selection = GetComponent<Selection>();
        rulesets = GetComponent<Rulesets>();
        functions = GetComponent<Functions>();
        pathfinding = GetComponent<Pathfinding>();
        main = GetComponent<Main>();
    }

    // Selection & pathfinding
    private void Update()
    {
        // Uses the "selection" script that already highlights a MorphBot to a different color (dark purple in this case) which shows where the mouse is pointing.
        // Once the user presses LMB and a MorphBot is not moving, the "selection" script is disabled, thus freezing the current MorphBot that we are staring at (which is highlighted purple).
        // This same MorphBot is then set to currentMorphBot and used for pathfinding.
        // ONLY WORKS IF FACING A MORPHBOT
        if (Input.GetMouseButtonDown(0) && isPathfinding == false)
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

            if (Physics.Raycast(ray, out raycastHit, maxDistance, gameLayers) && raycastHit.transform.gameObject.layer == 9)
            {
                selection.enabled = true;

                if (raycastHit.transform.gameObject == currentMorphBot && currentMorphBot != null)
                {
                    currentMorphBot = null;
                }

                else
                {

                    currentMorphBot = raycastHit.transform.gameObject;
                    selection.UnselectBlock();
                    selection.hoverMorphBot = currentMorphBot;
                    selection.SelectBlock();
                    selection.enabled = false;
                }
            }
        }

        // Stars pathfinding only if a block is selected and we are not pathfinding
        if (Input.GetKeyDown(translate) && currentMorphBot != null && isPathfinding == false)
        {
            Ray ray2;

            if (GetComponent<CameraManager>().currentMode == CameraManager.cameraMode.ROTATION)
            {
                ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            }

            else
            {
                ray2 = new Ray(GetComponent<CameraManager>().playerCamera.transform.position, GetComponent<CameraManager>().playerCamera.transform.forward);
            }

            if (Physics.Raycast(ray2, out raycastHit, maxDistance, gameLayers))
            {
                Vector3Int endLocation = functions.Vector3ToGrid(raycastHit);

                if (rulesets.WithinArray(endLocation))
                {
                    // Sets isPathfinding to true, disabling many features until a block is not moving (or does not find a path).
                    isPathfinding = true;
                    Vector3Int pos = Vector3Int.RoundToInt(currentMorphBot.transform.position);
                    main.grid[pos.x, pos.y, pos.z].walkable = true;
                    // References pathfinding and starts the moving process in the Pathfinding script.
                    pathfinding.FindPath(Vector3Int.RoundToInt(currentMorphBot.transform.position), endLocation);
                }
            }
        }
    }
}
