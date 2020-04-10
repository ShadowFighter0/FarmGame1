using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool editing;
    private readonly KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private int activeTool;
    public Transform tools;

    public GameObject indicator;
    public GameObject radialMenu;

    private bool radialMenuActive = false;

    public static InputManager instance;

    public enum States
    {
        Idle,
        Running,
        Working,
        OnUI,
        Sleeping
    };
    public static States state = States.Idle;
    private void Awake()
    {
        instance = this;
        UpdateStates();
    }

    private void Update()
    {
        if (Input.anyKey && state != States.OnUI && state != States.Sleeping)
        {
            if (Input.GetMouseButtonDown(1))
            {
                editing = !editing;
                indicator.SetActive(editing);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                editing = false;
                indicator.SetActive(editing);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                radialMenuActive = !radialMenuActive;
                if (!radialMenuActive)
                {
                    RadialMenuController.instance.Close();
                }
                else
                {
                    RadialMenuController.instance.Open();
                }
            }
        }
        int scroll = (int)Input.mouseScrollDelta.y;
        if (scroll != 0 && state != States.OnUI && state != States.Sleeping)
        {
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

    private static void UpdateStates()
    {
        switch (state)
        {
            case States.Idle:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                MovementController.instance.SetMovement(true);
                PlayerFollow.instance.SetMovement(true);
                break;
            case States.Working:
                break;
            case States.Running:

                break;
            case States.OnUI:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                MovementController.instance.SetMovement(false);
                PlayerFollow.instance.SetMovement(false);
                break;
            case States.Sleeping:
                MovementController.instance.SetMovement(false);
                PlayerFollow.instance.SetMovement(false);
                break;
        }
    }

    public static void ChangeState(States s) 
    { 
        state = s;
        UpdateStates();
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
