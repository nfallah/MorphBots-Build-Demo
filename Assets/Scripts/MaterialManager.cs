using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MaterialManager : MonoBehaviour
{
    public Material oldMaterial;

    public Material currentMaterial;

    [SerializeField] Material startingMaterial;

    [SerializeField] TextMeshProUGUI[] texts;

    [SerializeField] Image[] backgrounds;

    [SerializeField] Color unselected, selected;

    private Main main;

    private int index = 0;

    private void Awake()
    {
        currentMaterial = new Material(startingMaterial);
    }

    private void Start()
    {
        backgrounds[index].color = selected;
        GetComponent<Placement>().morphBotHover.GetComponent<MeshRenderer>().material = currentMaterial;
        main = GetComponent<Main>();
    }

    private void Update()
    {
        if (GetComponent<Main>().colorEnabled)
        {

            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && index < 2)
            {
                backgrounds[index].color = unselected;
                index++;
                backgrounds[index].color = selected;
            }

            else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && index > 0)
            {
                backgrounds[index].color = unselected;
                index--;
                backgrounds[index].color = selected;
            }

            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()) && int.Parse(texts[index].text + i) <= 255 && texts[index].text != "0")
                {
                    texts[index].text += i;
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && texts[index].text.Length > 0)
            {
                string str = "";

                for (int i = 0; i < texts[index].text.Length - 1; i++)
                {
                    str += texts[index].text[i];
                }

                texts[index].text = str;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                bool shouldStart = true;

                for (int i = 0; i <= 2; i++)
                {
                    if (texts[i].text.Length == 0)
                    {
                        shouldStart = false;
                        break;
                    }
                }

                if (shouldStart)
                {
                    currentMaterial.color = new Color(int.Parse(texts[0].text) / 255f, int.Parse(texts[1].text) / 255f, int.Parse(texts[2].text) / 255f, currentMaterial.color.a);
                    main.UpdateColor();
                    main.colorHelp.SetActive(false);
                }
            }
        }
    }
}
