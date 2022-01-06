using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    [SerializeField] GameObject clearChecker, sliderReset;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject cc = Instantiate(clearChecker);

            cc.GetComponent<ClearChecker>().flightPos = GetComponent<CameraManager>().flightPos;
            cc.GetComponent<ClearChecker>().ints[0] = GetComponent<Main>().x;
            cc.GetComponent<ClearChecker>().ints[1] = GetComponent<Main>().y - 1;
            cc.GetComponent<ClearChecker>().ints[2] = GetComponent<Main>().z;
            cc.GetComponent<ClearChecker>().flightRot = GetComponent<CameraManager>().flightRot;
            cc.GetComponent<ClearChecker>().rotationRot = GetComponent<CameraManager>().rotationRot;
            cc.GetComponent<ClearChecker>().currentMode = GetComponent<CameraManager>().currentMode;
            cc.GetComponent<ClearChecker>().cameraZoom = FindObjectOfType<Zoom>().currentZoom;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject sr = Instantiate(sliderReset);

            sr.GetComponent<SliderReset>().sensitivity = GetComponent<CameraManager>().sensitivity;
            sr.GetComponent<SliderReset>().blockSpeed = GetComponent<PathMovement>().initialTimer;
            sr.GetComponent<SliderReset>().movementSpeed = GetComponent<CameraManager>().movementSpeed;

            SceneManager.LoadScene("DimensionPicker");
        }
    }
}
