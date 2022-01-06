//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rulesets : MonoBehaviour
{
    // Was supposed to be a script that contained numerous rules that could be checked, but I moved them to Functions (except one).

    // References main script
    Main main;

    // Sets main script for later use.
    private void Awake()
    {
        main = GetComponent<Main>();
    }

    // Returns true if a Vector3Int location (a location that a script had found by snapping to the grid) is within the actual array).
    // Even if we are touching a morphBot or platform, we need to make sure it is within the wire cube in the editor. This helps with pathfinding, placement, movement, etc.
    public bool WithinArray(Vector3Int position)
    {
        if (position.x >= main.x || position.x < 0)
        {
            return false;
        }

        if (position.y >= main.y || position.y < 0)
        {
            return false;
        }

        if (position.z >= main.z || position.z < 0)
        {
            return false;
        }

        return true;
    }
}
