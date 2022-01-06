//Using the basic three
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    // IMPORTANT: the camera arm is a parent of the actual camera, which allows for rotation relative to a point (the camera arm's position)
    
    // The speed that the rotation of the camera will occur at
    public int rotationSpeed;

    // A Vector3 used to store, modify, and set the camera arm's rotation
    [HideInInspector] public Vector3 currentRotation;

    [SerializeField] Main main;

    // The maximum magnitude that the camera can rotate in the up/down direction
    public float downClamp, upClamp;

    // Rotation keys
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;

    // Makes sure that the camera arm's current rotation is within the magnitudes specified by rotationBounds
    // Also sets position of camera arm 
    private void Awake()
    {
        // Makes sure user input is a magnitude without negative signs
        downClamp = Mathf.Abs(downClamp);
        upClamp = Mathf.Abs(upClamp);
        currentRotation = this.transform.eulerAngles;
        // Moves the camera arm to the center of the platform (where the main camera will be rotating relative to)
        this.transform.position = new Vector3(FindObjectOfType<Main>().x / 2 - 0.5f, 0.5f, FindObjectOfType<Main>().z / 2 - 0.5f);

        if (FindObjectOfType<ClearChecker>() == null)
        {
            currentRotation = new Vector3(Mathf.Clamp(currentRotation.x, downClamp, upClamp), currentRotation.y, 0);
            this.transform.eulerAngles = currentRotation;
        }
    }

    private void Start()
    {
        if (FindObjectOfType<ClearChecker>() != null)
        {
            currentRotation = FindObjectOfType<ClearChecker>().cameraRotation;
            this.transform.eulerAngles = currentRotation;
            Destroy(FindObjectOfType<ClearChecker>().gameObject);
        }
    }

    // Runs a series of if/else statements to rotate the camera
    private void Update()
    {
        if (!main.colorEnabled)
        {
            if (Input.GetKey(up))
            {
                currentRotation.x = Mathf.Clamp(currentRotation.x + rotationSpeed * Time.deltaTime, downClamp, upClamp);
                this.transform.eulerAngles = currentRotation;
            }

            else if (Input.GetKey(down))
            {
                currentRotation.x = Mathf.Clamp(currentRotation.x - rotationSpeed * Time.deltaTime, downClamp, upClamp);
                this.transform.eulerAngles = currentRotation;
            }

            if (Input.GetKey(right))
            {
                currentRotation.y += rotationSpeed * Time.deltaTime;
                this.transform.eulerAngles = currentRotation;
            }

            else if (Input.GetKey(left))
            {
                currentRotation.y -= rotationSpeed * Time.deltaTime;
                this.transform.eulerAngles = currentRotation;
            }
        }
    }
}
