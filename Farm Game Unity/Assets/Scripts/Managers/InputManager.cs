using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool editing;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private int activeTool;
    public Transform tools;

    public GameObject indicator;
    public GameObject radialMenu;

    private bool radialMenuActive = false;

    public static InputManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        int scroll = (int)Input.mouseScrollDelta.y;
        if (Input.anyKey || scroll != 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                editing = !editing;
                indicator.SetActive(editing);
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                editing = false;
                indicator.SetActive(editing);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                radialMenuActive = !radialMenuActive;
                if(!radialMenuActive)
                {
                    RadialMenuController.instance.Close();
                }
                else
                {
                    RadialMenuController.instance.Open();
                }
            }
            if (radialMenuActive)
            {
                ChangeSeed(scroll);
            }
            else
            {
                ChangeTool(scroll);
            }
        }
    }

    private void ChangeSeed(int scroll)
    {
        int numSeeds = SeedPlanter.instance.GetSeedsLenght() - 1;
        for (int i = 0; i < numSeeds; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                SeedPlanter.instance.Index = i;
            }
        }

        if (scroll != 0)
        {
            SeedPlanter.instance.Index += scroll;
        }
    }

    private void ChangeTool(int scroll)
    {
        int oldActive = activeTool;
        int numTools = tools.transform.childCount - 1;
        for (int i = 0; i < numTools; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                activeTool = i;
            }
        }

        if (scroll != 0)
        {
            activeTool += scroll;
            if (activeTool < 0)
            {
                activeTool = numTools;
            }
            if (activeTool > numTools)
            {
                activeTool = 0;
            }
        }

        if (oldActive != activeTool)
        {
            tools.GetChild(oldActive).gameObject.SetActive(false);
            tools.GetChild(activeTool).gameObject.SetActive(true);
        }
    }
}
