using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
{
    public Transform parent;
    private GameObject[] icons;
    private RectTransform[] iconRects;
    private Vector2[] activePos;

    private int numChilds;

    public int distance;
    public float time = 0.2f;

    public Text text;

    public static RadialMenuController instance;

    void Awake()
    {
        instance = this;
        numChilds = parent.childCount;
        icons = new GameObject[numChilds];
        iconRects = new RectTransform[numChilds];

        int offset = 360 / numChilds;

        activePos = new Vector2[numChilds];

        for (int i = 0; i < numChilds; i++)
        {
            float angle = (offset * i) * Mathf.Deg2Rad;

            activePos[i].x = distance * Mathf.Cos(angle);
            activePos[i].y = distance * Mathf.Sin(angle);

            icons[i] = parent.GetChild(i).gameObject;
            iconRects[i] = parent.GetChild(i).GetComponent<RectTransform>();
        }
    }
    public void Open()
    {
        PlayerFollow.instance.SetMovement(false);
        parent.gameObject.SetActive(true);
        for (int i = 0; i < numChilds; i++)
        {
            if (InventoryController.Instance.GetAmount(SeedPlanter.instance.seeds[i].itemName) > 0)
            {
                icons[i].SetActive(true);
                iconRects[i].localPosition = activePos[i];
            }
        }
    }
    public void Close()
    {
        PlayerFollow.instance.SetMovement(true);
        parent.gameObject.SetActive(false);
        for (int i = 0; i < numChilds; i++)
        {
            icons[i].SetActive(false);
        }
        //StartCoroutine(Disable(exitButton));
    }
    public void ChangeText(string s)
    {
        text.text = s;
    }

    private IEnumerator Disable(GameObject go)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }
}
