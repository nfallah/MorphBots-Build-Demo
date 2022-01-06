//Using the basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    // These are a series of functions that are referenced and used throughout different scripts

    // Reference to "Main" and "Rulesets" scripts (everything is attached to the same GameObject)
    Main main;
    Rulesets rulesets;

    // Sets reference script variables
    private void Awake()
    {
        main = GetComponent<Main>();
        rulesets = GetComponent<Rulesets>();
    }

    // Used to convert a raycast.point Vector3 into a Vector3Int that is snapped to a grid (used for placement, etc)
    // e.g (0.4, -1.4, 2.7) ----> (0, -1, 3)
    // Trunucate would do the exact same thing as the floor function but better (could not find it though)
    public Vector3Int Vector3ToGrid(RaycastHit raycastHit)
    {
        Vector3 rawPosition = raycastHit.point - raycastHit.transform.position;
        Vector3 truePosition = new Vector3(0, 0, 0);

        if (rawPosition.x > 0)
        {
            truePosition.x = Mathf.Floor(rawPosition.x + 0.5f);
        }

        else if (rawPosition.x < 0)
        {
            truePosition.x = Mathf.Floor(rawPosition.x * -1 + 0.5f) * -1;
        }

        if (rawPosition.y > 0)
        {
            truePosition.y = Mathf.Floor(rawPosition.y + 0.5f);
        }

        else if (rawPosition.y < 0)
        {
            truePosition.y = Mathf.Floor(rawPosition.y * -1 + 0.5f) * -1;
        }

        if (rawPosition.z > 0)
        {
            truePosition.z = Mathf.Floor(rawPosition.z + 0.5f);
        }

        else if (rawPosition.z < 0)
        {
            truePosition.z = Mathf.Floor(rawPosition.z * -1 + 0.5f) * -1;
        }

        truePosition += raycastHit.transform.position;
        return Vector3Int.RoundToInt(truePosition);
    }

    // A pathfinding function used by A* to find neighbors (empty places in the grid the block can move to)
    // It must abide by the MorphBots' physical limitations (e.g no floating in the air, must always be touching two blocks in the direction that it is traveling)
    // References other pathfinding functions below such as EmptyCheck
    // Neighbors cannot be diagonal
    public List<Node> GetNeighbors(Node currentNode)
    {
        List<Node> Neighbors = new List<Node>();
        int x = currentNode.position.x;
        int y = currentNode.position.y;
        int z = currentNode.position.z;

        if (rulesets.WithinArray(currentNode.position + Vector3Int.right) && main.grid[x + 1, y, z].walkable == true)
        {
            if (EmptyCheckX(true, currentNode.position))
            {
                Neighbors.Add(main.grid[x + 1, y, z]);
            }
        }

        if (rulesets.WithinArray(currentNode.position + Vector3Int.left) && main.grid[x - 1, y, z].walkable == true)
        {
            if (EmptyCheckX(false, currentNode.position))
            {
                Neighbors.Add(main.grid[x - 1, y, z]);
            }
        }

        if (rulesets.WithinArray(currentNode.position + Vector3Int.RoundToInt(Vector3.forward)) && main.grid[x, y, z + 1].walkable == true)
        {
            if (EmptyCheckZ(true, currentNode.position))
            {
                Neighbors.Add(main.grid[x, y, z + 1]);
            }
        }

        if (rulesets.WithinArray(currentNode.position + Vector3Int.RoundToInt(Vector3.back)) && main.grid[x, y, z - 1].walkable == true)
        {
            if (EmptyCheckZ(false, currentNode.position))
            {
                Neighbors.Add(main.grid[x, y, z - 1]);
            }
        }

        if (rulesets.WithinArray(currentNode.position + Vector3Int.up) && main.grid[x, y + 1, z].walkable == true)
        {
            if (EmptyCheckY(true, currentNode.position))
            {
                Neighbors.Add(main.grid[x, y + 1, z]);
            }
        }

        if (rulesets.WithinArray(currentNode.position + Vector3Int.down) && main.grid[x, y - 1, z].walkable == true)
        {
            if (EmptyCheckY(false, currentNode.position))
            {
                Neighbors.Add(main.grid[x, y - 1, z]);
            }
        }

        return Neighbors;
    }

    // Checks if there are 2 CONSECUTIVE placed MorphBots in the UP, DOWN, FORWARD, or BACK directions
    // Depends on whether a MorphBot is moving to the LEFT or RIGHT
    // A MorphBot must always be touching 2 blocks in order to move, whether it be the platform or other MorphBots
    // Used in GetNeighbors script to find valid neighbors to move to
    public bool EmptyCheckX(bool isRight, Vector3Int pos)
    {
        int sign;

        if (isRight)
        {
            sign = 1;
        }

        else
        {
            sign = -1;
        }

        if (rulesets.WithinArray(pos + Vector3Int.up) && main.grid[pos.x, pos.y + 1, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.up + new Vector3Int(sign, 0, 0)) && main.grid[pos.x + sign, pos.y + 1, pos.z].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.down) && main.grid[pos.x, pos.y - 1, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.down + new Vector3Int(sign, 0, 0)) && main.grid[pos.x + sign, pos.y - 1, pos.z].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.forward)) && main.grid[pos.x, pos.y, pos.z + 1].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.forward) + new Vector3Int(sign, 0, 0)) && main.grid[pos.x + sign, pos.y, pos.z + 1].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.back)) && main.grid[pos.x, pos.y, pos.z - 1].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.back) + new Vector3Int(sign, 0, 0)) && main.grid[pos.x + sign, pos.y, pos.z - 1].walkable == false)
            {
                return true;
            }
        }

        return false;
    }

    // Checks if there are 2 CONSECUTIVE placed MorphBots in the LEFT, RIGHT, FORWARD, or BACK directions
    // Depends on whether a MorphBot is moving UP or DOWN
    // A MorphBot must always be touching 2 blocks in order to move, whether it be the platform or other MorphBots
    // Used in GetNeighbors script to find valid neighbors to move to
    public bool EmptyCheckY(bool isUp, Vector3Int pos)
    {
        int sign;

        if (isUp)
        {
            sign = 1;
        }

        else
        {
            sign = -1;
        }

        if (rulesets.WithinArray(pos + Vector3Int.right) && main.grid[pos.x + 1, pos.y, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.right + new Vector3Int(0, sign, 0)) && main.grid[pos.x + 1, pos.y + sign, pos.z].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.left) && main.grid[pos.x - 1, pos.y, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.left + new Vector3Int(0, sign, 0)) && main.grid[pos.x - 1, pos.y + sign, pos.z].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.forward)) && main.grid[pos.x, pos.y, pos.z + 1].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.forward) + new Vector3Int(0, sign, 0)) && main.grid[pos.x, pos.y + sign, pos.z + 1].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.back)) && main.grid[pos.x, pos.y, pos.z - 1].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.RoundToInt(Vector3.back) + new Vector3Int(0, sign, 0)) && main.grid[pos.x, pos.y + sign, pos.z - 1].walkable == false)
            {
                return true;
            }
        }

        return false;
    }

    // Checks if there are 2 CONSECUTIVE placed MorphBots in the LEFT, RIGHT, UP, or DOWN directions
    // Depends on whether a MorphBot is moving to the LEFT or RIGHT
    // A MorphBot must always be touching 2 blocks in order to move, whether it be the platform or other MorphBots
    // Used in GetNeighbors script to find valid neighbors to move to
    public bool EmptyCheckZ(bool isForward, Vector3Int pos)
    {
        int sign;

        if (isForward)
        {
            sign = 1;
        }

        else
        {
            sign = -1;
        }

        if (rulesets.WithinArray(pos + Vector3Int.right) && main.grid[pos.x + 1, pos.y, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.right + new Vector3Int(0, 0, sign)) && main.grid[pos.x + 1, pos.y, pos.z + sign].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.left) && main.grid[pos.x - 1, pos.y, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.left + new Vector3Int(0, 0, sign)) && main.grid[pos.x - 1, pos.y, pos.z + sign].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.up) && main.grid[pos.x, pos.y + 1, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.up + new Vector3Int(0, 0, sign)) && main.grid[pos.x, pos.y + 1, pos.z + sign].walkable == false)
            {
                return true;
            }
        }

        if (rulesets.WithinArray(pos + Vector3Int.down) && main.grid[pos.x, pos.y - 1, pos.z].walkable == false)
        {
            if (rulesets.WithinArray(pos + Vector3Int.down + new Vector3Int(0, 0, sign)) && main.grid[pos.x, pos.y - 1, pos.z + sign].walkable == false)
            {
                return true;
            }
        }

        return false;
    }
}
