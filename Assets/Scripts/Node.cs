//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // This is a class that is used for the "grid" multidimensional array, referencing every Vector3Int location in that grid.

    // Position of Node
    public Vector3Int position;
    
    // States whether this Node is walkable (a MorphBot can move on it)
    public bool walkable;

    // Distance from the start Node
    public int gCost;

    // Distance from the end Node
    public int hCost;

    // gCost + hCost
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }


    // Parent Node used in pathfinding to backtrack
    public Node parent;

    // Constructable used in 'Main' script to alter position and walkable variables
    public Node(Vector3Int _position, bool _walkable)
    {
        position = _position;
        walkable = _walkable;
    }
}
