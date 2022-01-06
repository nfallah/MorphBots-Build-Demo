using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] Slider sensSlider, moveSlider, blockSlider;

    [SerializeField] float sensMultiplier, moveMultiplier, blockMultiplier;

    public float sens, move, block;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        if (FindObjectOfType<SliderReset>() != null)
        {
            SliderReset sr = FindObjectOfType<SliderReset>();

            sensSlider.value = sr.sensitivity / sensMultiplier;
            moveSlider.value = sr.movementSpeed / moveMultiplier;
            blockSlider.value = (sr.blockSpeed - (blockMultiplier * blockSlider.maxValue) - blockMultiplier) / -blockMultiplier;

            Destroy(sr.gameObject);
        }

        DontDestroyOnLoad(this);
        UpdateSens();
        UpdateMove();
        UpdateBlock();
    }

    public void UpdateSens()
    {
        sens = sensSlider.value * sensMultiplier;
    }

    public void UpdateMove()
    {
        move = moveSlider.value * moveMultiplier;
    }

    public void UpdateBlock()
    {
        block = blockMultiplier * (blockSlider.maxValue + 1 - blockSlider.value);
    }
}
