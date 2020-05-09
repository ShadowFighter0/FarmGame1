using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page
{
    public GameObject parent;
    public Image[] images;

    public Page(GameObject p, GameObject[] icons, Image[] spr, Vector2[] pos)
    {
        parent = p;
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].GetComponent<RectTransform>().localPosition = pos[i];
        }
        images = spr;
    }
    public void OpenChilds()
    {
        parent.SetActive(true);
    }
    public void CloseChilds()
    {
        parent.SetActive(false);
    }
}
public class RadialMenuController : MonoBehaviour
{
    public GameObject parent;
    public GameObject changeButton;

    public Sprite def;

    public int distance;
    public float time = 0.2f;

    public Text text;

    public static RadialMenuController instance;
    private readonly Page[] page= new Page[2];

    private int currentPage = 0;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < 2; i++)
        {
            int numChilds = parent.transform.GetChild(i).childCount;
            GameObject[] icons = new GameObject[numChilds];
            Image[] images = new Image[numChilds];
            Vector2[] activePos = new Vector2[numChilds];

            int offset = 360 / numChilds;
            for (int j = 0; j < numChilds; j++)
            {
                float angle = (offset * j) * Mathf.Deg2Rad;

                activePos[j].x = distance * Mathf.Cos(angle);
                activePos[j].y = distance * Mathf.Sin(angle);

                icons[j] = parent.transform.GetChild(i).GetChild(j).gameObject;
                images[j] = icons[j].GetComponent<Image>();
            }
            page[i] = new Page(parent.transform.GetChild(i).gameObject, icons, images, activePos);
        }
    }
    private void Start()
    {
        FillPages();
    }
    public void FillPages()
    {
        List<Seed> seeds = SeedPlanter.instance.CurrentSeeds();
        int numSeeds = 0;
        for (int i = 0; i < 2; i++)
        {
            int num = parent.transform.GetChild(i).childCount;
            for (int j = 0; j < num; j++)
            {
                if(numSeeds < seeds.Count)
                {
                    numSeeds++;
                    page[i].images[j].sprite = seeds[j].image;
                }
                else
                {
                    page[i].images[j].sprite = def;
                }
            }
        }
    }
    public void SetPage(int i) { currentPage = i; }

    public void ChangePage()
    {
        page[currentPage].CloseChilds();
        if (currentPage == 0)
        {
            currentPage = 1;
        }
        else
        {
            currentPage = 0;
        }
        page[currentPage].OpenChilds();
    }
    public void Open()
    {
        FillPages();
        parent.SetActive(true);
        changeButton.SetActive(true);
        page[currentPage].OpenChilds();
    }
    public void Close()
    {
        parent.SetActive(false);
        changeButton.SetActive(false);
        page[currentPage].CloseChilds();
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
    public void ChangeText(string s)
    {
        text.text = s;
    }

    public void SetSeed(Transform t)
    {
        int index = t.GetSiblingIndex();
        if(index <= SeedPlanter.instance.GetCurrentSeeds())
        {
            SeedPlanter.instance.Index = index;
            Close();
        }
    }
}
