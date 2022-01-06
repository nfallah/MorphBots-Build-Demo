//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Material beforeMatOld, beforeMatNew;
    GameObject beforeGameObject;
    // The darker material that shows a mouse is touching a MorphBot
    public Material hover;

    // The normal material that a morphBot is turned back into if the mouse is no longer touching it
    public Material defaultMat;

    // The results of the below raycast (if it was successful).
    RaycastHit raycastHit;

    // Max distance of the raycast.
    public float maxDistance;

    // The layers used for the raycast (platform, MorphBot)
    public LayerMask gameLayers;

    // The current dark morphBot that the mouse is pointing at. It is used mainly in the Movement script to select a morphBot that can be moved with pathfinding.
    public GameObject hoverMorphBot;

    // Does a Raycast. If something in gameLayers is hit, and that thing is a morphBot, then that GameObject's material will be turned into a different material, indiating the mouse is pointing at it.
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

        if (Physics.Raycast(ray, out raycastHit, maxDistance, gameLayers))
        {
            if (raycastHit.transform.gameObject.layer == 9)
            {
                if (raycastHit.transform.gameObject != hoverMorphBot)
                {
                    if (hoverMorphBot != null)
                    {
                        UnselectBlock();
                    }

                    hoverMorphBot = raycastHit.transform.gameObject;
                    SelectBlock();
                }
            }

            else
            {
                if (hoverMorphBot != null)
                {
                    UnselectBlock();
                    hoverMorphBot = null;
                }
            }
        }

        else
        {
            if (hoverMorphBot != null)
            {
                {
                    UnselectBlock();
                    hoverMorphBot = null;
                }
            }
        }
    }

 // Changes materials accordingly
    public void SelectBlock()
    {
        Material mat = hoverMorphBot.GetComponent<MeshRenderer>().material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.5f);
        hoverMorphBot.GetComponent<MeshRenderer>().material = mat;
    }

    public void UnselectBlock()
    {
        Material mat = hoverMorphBot.GetComponent<MeshRenderer>().material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        hoverMorphBot.GetComponent<MeshRenderer>().material = mat;
    }
}
