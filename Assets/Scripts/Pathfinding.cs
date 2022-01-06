//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // References other scripts
    Main main;
    Functions functions;
    Movement movement;
    PathMovement pathMovement;

    // Stores every Node in order if a path is found
    public List<Node> pathfinder;

    // Returns true or false depending on whether a path was found
    public bool hasFoundPath;

    // Sets reference script variables
    private void Awake()
    {
        main = GetComponent<Main>();
        functions = GetComponent<Functions>();
        movement = GetComponent<Movement>();
        pathMovement = GetComponent<PathMovement>();
    }

    // A lot of this is hard to explain, but the code is pretty much identical to the A* video by Sebastian Lague (part 3)
    public void FindPath(Vector3Int startPos, Vector3Int endPos)
    {
        hasFoundPath = false;
        Node startNode = main.grid[startPos.x, startPos.y, startPos.z];
        Node endNode = main.grid[endPos.x, endPos.y, endPos.z];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1;  i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                hasFoundPath = true;
                RetracePath(startNode, endNode);
                pathMovement.tempPos = startPos;
                pathMovement.index = 0;
                pathMovement.MoveCube();
                Debug.Log("PATH HAS BEEN FOUND");
            }

            foreach (Node neighbor in functions.GetNeighbors(currentNode))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCost = currentNode.gCost + CalculateCost(currentNode, neighbor);
                
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = CalculateCost(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // If no path has been found, then things are changed as accordingly.
        // isPathfinding is set to false once again, meaning the user can input a new location to test.
        // The starting Node is now walkable again because it is not going to move, meaning it will be occupied.
        if (!hasFoundPath)
        {
            Debug.Log("PATH NOT FOUND");
            movement.isPathfinding = false;
            main.grid[startPos.x, startPos.y, startPos.z].walkable = false;
        }
    }

    // Calculates the movement cost to a given Node.
    private int CalculateCost(Node nodeA, Node nodeB)
    {
        Vector3Int posA = nodeA.position;
        Vector3Int posB = nodeB.position;

        int xCost = Mathf.Abs(posB.x - posA.x);
        int yCost = Mathf.Abs(posB.y - posA.y);
        int zCost = Mathf.Abs(posB.z - posA.z);

        return xCost + yCost + zCost;
    }

    // Retraces a path once we have confirmed there is one.
    // Sets pathfinder so that we can start moving.
    private void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        pathfinder = path;
    }
}
