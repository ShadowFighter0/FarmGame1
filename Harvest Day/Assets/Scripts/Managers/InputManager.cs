using System.Collections;
using System.Collections.Generic;
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
    public KeyCode Escape;

    public KeyCode InteractControl;
    public KeyCode WorkControl;
    public KeyCode RadialMenuControl;
    public KeyCode InventoryControl;
    public KeyCode RunControl;
    public KeyCode ClickControl;
    public KeyCode EscapeControl;

    private int activeTool = 0;
    private int oldActive = 0;

    private Transform toolsFolder;
    public GameObject radialMenu;

    private GameObject[] tools;

    public Animator playerAnim;

    private bool canChangeTool = true;

    public static InputManager instance;

    public enum States
    {
        Idle,
        SelectingSeed,
        Working,
        OnUI,
        Dialoguing,
        Sleeping,
        Editing
    };
    public static States state = States.OnUI;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InteractControl = KeyCode.JoystickButton1;
        WorkControl = KeyCode.JoystickButton7;
        RadialMenuControl = KeyCode.JoystickButton6;
        InventoryControl = KeyCode.JoystickButton3;
        RunControl = KeyCode.JoystickButton10;
        ClickControl = KeyCode.JoystickButton7;
        EscapeControl = KeyCode.JoystickButton2;


        if (Application.isEditor)
        {
            Escape = KeyCode.F1;
        }
        else
        {
            Escape = KeyCode.Escape;
        }
        
        UpdateStates();
        tools = new GameObject[toolsFolder.childCount];
        for (int i = 0; i < toolsFolder.childCount; i++)
        {
            tools[i] = toolsFolder.GetChild(i).gameObject;
        }

    }

    private void Update()
    {
        int scroll = (int)Input.mouseScrollDelta.y;

        switch (state)
        {
            case States.Idle:
                if (Input.anyKeyDown || scroll != 0)
                {
                    if (Input.GetKeyDown(RadialMenu) || Input.GetKeyDown(RadialMenuControl))
                    {
                        ChangeState(States.SelectingSeed);
                        RadialMenuController.instance.Open();
                    }
                    if (Input.GetKeyDown(Inventory) || Input.GetKeyDown(InventoryControl))
                    {
                        InventoryController.Instance.OpenMenu();
                    }
                    ChangeTool(scroll);
                }
                break;
            case States.Working:
                break;
            case States.SelectingSeed:
                if ((Input.anyKeyDown || scroll != 0))
                {
                    if (Input.GetKeyDown(RadialMenu) || Input.GetKeyDown(RadialMenuControl))
                    {
                        ChangeState(States.Idle);
                        RadialMenuController.instance.Close();
                    }
                    ChangeSeed(scroll);
                }
                    
                break;
            case States.OnUI:
                if ((Input.GetKeyDown(Inventory) || Input.GetKeyDown(InventoryControl)) && InventoryController.Instance.inventoryOpen)
                {
                    InventoryController.Instance.CloseMenu();
                }
                break;
            case States.Dialoguing:

                break;
            case States.Sleeping:

                break;
        }
    }

    public void SetCanChangeTool(bool b) { canChangeTool = b; }
    public void SetAnimator(Animator anim) { playerAnim = anim; }
    private void UpdateStates()
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
                MovementController.instance.SetMovement(false);
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
            case States.Editing:
                ShowCursorBlockMovement();
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
        state = s;
        UpdateStates();
    }
    private void ChangeSeed(int scroll)
    {
        if (scroll != 0)
        {
            SeedPlanter.instance.Index += scroll;
            SeedPlanter.instance.UpdateIndicator();
        }
    }
    public void ChangeVisualTool()
    {
        tools[oldActive].gameObject.SetActive(false);
        tools[activeTool].gameObject.SetActive(true);
    }
    public void Unnequip()
    {
        tools[activeTool].SetActive(!tools[activeTool].activeSelf);
    }
    public void SetTools(Transform t) { toolsFolder = t; }

    private void ChangeTool(int scroll)
    {
        if (canChangeTool)
        {
            oldActive = activeTool;
            int numTools = toolsFolder.transform.childCount;

            if (Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                activeTool = 0;
                if (oldActive == activeTool)
                {
                    //unequip aniamtion
                    playerAnim.SetTrigger("Unnequip");
                }
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                activeTool = 1;
                if (oldActive == activeTool)
                {
                    //unequip aniamtion
                    playerAnim.SetTrigger("Unnequip");
                }
            }

            for (int i = 0; i < numTools; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    activeTool = i;
                    if (oldActive == activeTool)
                    {
                        //unequip aniamtion
                        playerAnim.SetTrigger("Unnequip");
                    }
                }
            }

            if (scroll != 0)
            {
                activeTool += scroll;
                if (activeTool < 0)
                {
                    activeTool = numTools - 1;
                }
                if (activeTool > numTools - 1)
                {
                    activeTool = 0;
                }
            }

            if (oldActive != activeTool)
            {
                //equip animation
                playerAnim.SetTrigger("ChangeTool");
            }
        }
    }
}
