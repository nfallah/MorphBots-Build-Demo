using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [SerializeField] float xSpeed, ySpeed;

    private void Start()
    {
        if (Random.Range(0, 1f) > 0.5f)
        {
            xSpeed *= -1;
        }

        if (Random.Range(0, 1f) > 0.5f)
        {
            ySpeed *= -1;
        }

        xSpeed *= Random.Range(1, 5);
        ySpeed *= Random.Range(1, 5);
    }

    private void Update()
    {
        this.transform.eulerAngles += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;

        if (this.transform.eulerAngles.x >= 90)
        {
            xSpeed *= -1;
        }
    }
}
