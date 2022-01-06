using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum cameraMode { FLIGHT, ROTATION }

    public cameraMode currentMode;

    public Camera playerCamera;

    [SerializeField] GameObject cameraArm, crosshair;

    public Vector3 flightPos, flightRot, rotationRot;

    public float rotationSpeed, downClamp, upClamp, sensitivity, movementSpeed;

    private bool canUpdate = false;

    private void Start()
    {
        cameraArm.transform.position = new Vector3(FindObjectOfType<Main>().x / 2 - 0.5f, 0.5f, FindObjectOfType<Main>().z / 2 - 0.5f);

        if (FindObjectOfType<SliderManager>() != null)
        {
            sensitivity = FindObjectOfType<SliderManager>().sens;
            movementSpeed = FindObjectOfType<SliderManager>().move;

            Destroy(FindObjectOfType<SliderManager>().gameObject);
        }

        if (FindObjectOfType<ClearChecker>() == null)
        {
            flightPos = new Vector3(FindObjectOfType<Main>().x / 2 - 0.5f, 3f, -5);
            flightRot = Vector3.zero;
            rotationRot = new Vector3(25, 0, 0);
        }

        else
        {
            ClearChecker cc = FindObjectOfType<ClearChecker>();

            flightRot = cc.flightRot;
            flightPos = cc.flightPos;
            rotationRot = cc.rotationRot;
            currentMode = cc.currentMode;

            Destroy(cc.gameObject);
        }

        UpdateMode();
    }

    private void Update()
    {
        if (!GetComponent<Main>().colorEnabled && canUpdate)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SwitchMode();
                UpdateMode();
            }

            switch (currentMode)
            {
                case cameraMode.FLIGHT:
                    float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                    float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

                    flightRot = new Vector3(Mathf.Clamp(flightRot.x - y, -90, 90), flightRot.y + x, 0);
                    playerCamera.transform.eulerAngles = flightRot;

                    float moveX = Input.GetAxisRaw("Horizontal");
                    float moveZ = Input.GetAxisRaw("Vertical");

                    Vector3 movement = (playerCamera.transform.right * moveX + playerCamera.transform.forward * moveZ) * Time.deltaTime * movementSpeed;

                    playerCamera.GetComponent<CharacterController>().Move(movement);

                    flightPos = playerCamera.transform.position;

                    flightPos = new Vector3(Mathf.Clamp(flightPos.x, -15, GetComponent<Main>().x + 14), Mathf.Clamp(flightPos.y, -15, GetComponent<Main>().y + 14), Mathf.Clamp(flightPos.z, -15, GetComponent<Main>().z + 14));
                    playerCamera.transform.position = flightPos;

                    break;

                case cameraMode.ROTATION:
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    {
                        rotationRot.x = Mathf.Clamp(rotationRot.x + rotationSpeed * Time.deltaTime, downClamp, upClamp);
                        cameraArm.transform.eulerAngles = rotationRot;
                    }

                    else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        rotationRot.x = Mathf.Clamp(rotationRot.x - rotationSpeed * Time.deltaTime, downClamp, upClamp);
                        cameraArm.transform.eulerAngles = rotationRot;
                    }

                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        rotationRot.y = (rotationRot.y + rotationSpeed * Time.deltaTime) % 360;
                        cameraArm.transform.eulerAngles = rotationRot;
                    }

                    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        rotationRot.y = (rotationRot.y - rotationSpeed * Time.deltaTime) % 360;
                        cameraArm.transform.eulerAngles = rotationRot;
                    }

                    break;
            }
        }
    }

    private void SwitchMode()
    {
        switch (currentMode)
        {
            case cameraMode.FLIGHT:
                currentMode = cameraMode.ROTATION;
                break;

            case cameraMode.ROTATION:
                currentMode = cameraMode.FLIGHT;
                break;
        }
    }

    private void UpdateMode()
    {
        switch (currentMode)
        {
            case cameraMode.FLIGHT:
                crosshair.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                playerCamera.transform.eulerAngles = flightRot;
                playerCamera.transform.position = flightPos;
                FindObjectOfType<Zoom>().enabled = false;
                playerCamera.GetComponent<CharacterController>().enabled = true;
                if (!canUpdate)
                {
                    Invoke("CanUpdate", Time.deltaTime * 10);
                }
                break;

            case cameraMode.ROTATION:
                crosshair.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                playerCamera.transform.localEulerAngles = Vector3.zero;
                playerCamera.transform.localPosition = FindObjectOfType<Zoom>().currentZoom;
                cameraArm.transform.eulerAngles = rotationRot;
                FindObjectOfType<Zoom>().enabled = true;
                playerCamera.GetComponent<CharacterController>().enabled = false;
                if (!canUpdate)
                {
                    Invoke("CanUpdate", Time.deltaTime * 10);
                }
                break;
        }
    }

    private void CanUpdate()
    {
        canUpdate = true;
    }
}