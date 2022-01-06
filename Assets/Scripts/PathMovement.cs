//Uses basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    // Reference to other scripts
    Movement movement;
    Main main;
    Pathfinding pathfinding;

    // How long it takes for a MorphBot to move 1 unit
    public float initialTimer;

    // Reference to initialTimer
    float currentTimer;

    // Reprecents currentTimer / initialTimer as a percent for Vector3Lerp
    float timePercent;

    // The index of the pathfinder Node list
    public int index;

    // A Vector3Int (although not literally)
    // It will stay as (1, 1, 4) until it gets to (1, 1, 5) [an example destination]. Even if the MorphBot's current location is (1, 1, 4.99), tempPos will not change. 
    // Once it reaches (1, 1, 5), then tempPos will also change to (1, 1, 5). Now it will travel to (1, 1, 6).
    public Vector3 tempPos;

    // Sets currentTimer to initialTimer (currentTimer is the same amount of time, but is deducted from every frame. InitialTimer is a constant that is often referenced).
    // Also sets referenced script variables for later use
    private void Awake()
    {
        if (FindObjectOfType<SliderManager>() != null)
        {
            initialTimer = FindObjectOfType<SliderManager>().block; 
        }

        movement = GetComponent<Movement>();
        main = GetComponent<Main>();
        pathfinding = GetComponent<Pathfinding>();
        currentTimer = initialTimer;
    }

    // Keeps subtracting from currentTime time every frame, using currentTime / initialTime as a percent which is then used to Vector3Lerp from one step to another. This repeats until the current location of
    // the MorphBot is the same as its destination.
    public void MoveCube()
    {
        movement.currentMorphBot.transform.position = Vector3.Lerp(tempPos, pathfinding.pathfinder[index].position, Timer());

        if (movement.currentMorphBot.transform.position == pathfinding.pathfinder[index].position)
        {
            movement.currentMorphBot.transform.position = pathfinding.pathfinder[index].position;
            tempPos = movement.currentMorphBot.transform.position;
            currentTimer = initialTimer;
            index++;
        }

        if (movement.currentMorphBot.transform.position != pathfinding.pathfinder[pathfinding.pathfinder.Count - 1].position)
        {
            Invoke("MoveCube", Time.deltaTime);
        }

        else
        {
            Vector3Int newPos = Vector3Int.RoundToInt(tempPos);
            movement.isPathfinding = false;
            main.grid[newPos.x, newPos.y, newPos.z].walkable = false;
            movement.currentMorphBot.name = "MorphBot(" + newPos.x + ", " + newPos.y + ", " + newPos.z + ")";
            print("FINISHED");
        }
    }

    private float Timer()
    {
        currentTimer = Mathf.Clamp(currentTimer - Time.deltaTime, 0, initialTimer);
        timePercent = 1 - (currentTimer / initialTimer);

        return timePercent;

    }
}
