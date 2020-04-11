using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private readonly KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    public KeyCode Interact;
    public KeyCode Work;
    public KeyCode RadialMenu;
    public KeyCode Inventory;
    public KeyCode Run;
    public KeyCode Click;

    private int activeTool;
    public Transform tools;

    public GameObject indicator;
    public GameObject radialMenu;

    public static InputManager instance;

    public enum States
    {
        Idle,
        SelectingSeed,
        Working,
        OnUI,
        Dialoguing,
        Sleeping
    };
    public static States state = States.Idle;
    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        UpdateStates();
    }

    private void Update()
    {
        int scroll = (int)Input.mouseScrollDelta.y;
        switch (state)
        {
            case States.Idle:
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(RadialMenu))
                    {
                        ChangeState(States.SelectingSeed);
                        RadialMenuController.instance.Open();
                    }
                    if(Input.GetKeyDown(Work))
                    {
                        ChangeState(States.Working);
                    }
                }
                break;
            case States.Working:
                if ((Input.anyKey || scroll != 0))
                {
                    ChangeTool(scroll);
                    if (Input.GetKeyDown(Work) || Input.GetKeyDown(Run))
                    {
                        ChangeState(States.Idle);
                    }
                }
                break;
            case States.SelectingSeed:
                if ((Input.anyKey || scroll != 0))
                {
                    if (Input.GetKeyDown(RadialMenu))
                    {
                        ChangeState(States.Idle);
                        RadialMenuController.instance.Close();
                    }
                    ChangeSeed(scroll);
                }
                    
                break;
            case States.OnUI:

                break;
            case States.Dialoguing:

                break;
            case States.Sleeping:

                break;
        }

    }

    private void UpdateStates()
    {
        switch (state)
        {
            case States.Idle:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                MovementController.instance.SetMovement(true);
                PlayerFollow.instance.SetMovement(true);
                indicator.SetActive(false);
                break;
            case States.Working:
                indicator.SetActive(true);
                break;
            case States.SelectingSeed:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                PlayerFollow.instance.SetMovement(false);
                break;
            case States.OnUI:
                ShowCursorBlockMovement();
                break;
            case States.Dialoguing:
                ShowCursorBlockMovement();
                break;
            case States.Sleeping:
                MovementController.instance.SetMovement(false);
                PlayerFollow.instance.SetMovement(false);
                break;
        }
    }

    private static void ShowCursorBlockMovement()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MovementController.instance.SetMovement(false);
        PlayerFollow.instance.SetMovement(false);
    }

    public void ChangeState(States s) 
    {
        Debug.Log("State: " + s);
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
