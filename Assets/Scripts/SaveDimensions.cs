using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveDimensions : MonoBehaviour
{
    [SerializeField] Color selected, unselected;
    [SerializeField] int maxNumber;
    [SerializeField] TextMeshProUGUI x, y, z;

    int currentIndex;
    TextMeshProUGUI[] texts;
    public string[] tStrings;
    string xT, yT, zT;
    public Image xImg, yImg, zImg;
    Image[] images;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        texts = new TextMeshProUGUI[3];
        images = new Image[3];
        tStrings = new string[3];

        xT = yT = zT = "";

        tStrings[0] = xT;
        tStrings[1] = yT;
        tStrings[2] = zT;

        images[0] = xImg;
        images[1] = yImg;
        images[2] = zImg;

        texts[0] = x;
        texts[1] = y;
        texts[2] = z;

        currentIndex = 0;

        images[currentIndex].color = selected;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && currentIndex < 2)
        {
            images[currentIndex].color = unselected;
            currentIndex += 1;
            images[currentIndex].color = selected;
        }

        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && currentIndex > 0)
        {
            images[currentIndex].color = unselected;
            currentIndex -= 1;
            images[currentIndex].color = selected;
        }

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(i.ToString()) && int.Parse(tStrings[currentIndex] + i) <= maxNumber)
            {
                if (i != 0 || tStrings[currentIndex].Length != 0)
                {
                    tStrings[currentIndex] += i;
                    texts[currentIndex].text = tStrings[currentIndex];
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && tStrings[currentIndex].Length > 0)
        {
            string newString = "";

            for (int i = 0; i < tStrings[currentIndex].Length - 1; i++)
            {
                newString += tStrings[currentIndex][i];
            }

            tStrings[currentIndex] = newString;
            texts[currentIndex].text = tStrings[currentIndex];
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
