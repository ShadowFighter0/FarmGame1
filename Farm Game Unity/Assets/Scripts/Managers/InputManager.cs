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
    public KeyCode Escape;

    private int activeTool = 0;
    private Transform tools;
    public GameObject radialMenu;

    public Animator playerAnim;

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
        UpdateStates();
    }

    private void Update()
    {
        int scroll = (int)Input.mouseScrollDelta.y;
        switch (state)
        {
            case States.Idle:
                if (Input.anyKeyDown || scroll != 0)
                {
                    if (Input.GetKeyDown(RadialMenu))
                    {
                        ChangeState(States.SelectingSeed);
                        RadialMenuController.instance.Open();
                    }
                    if (Input.GetKeyDown(Inventory))
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
                    if (Input.GetKeyDown(RadialMenu))
                    {
                        ChangeState(States.Idle);
                        RadialMenuController.instance.Close();
                    }
                    ChangeSeed(scroll);
                }
                    
                break;
            case States.OnUI:
                if (Input.GetKeyDown(Inventory))
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
        Debug.Log("State: " + state.ToString());
        UpdateStates();
    }
    private void ChangeSeed(int scroll)
    {
        if (scroll != 0)
        {
            SeedPlanter.instance.Index += scroll;
        }
    }

    public void SetTools(Transform t) { tools = t; }

    private void ChangeTool(int scroll)
    {
        int oldActive = activeTool;
        int numTools = tools.transform.childCount;
        for (int i = 0; i < numTools; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                activeTool = i;

                if (oldActive == activeTool)
                {
                    GameObject go = tools.GetChild(activeTool).gameObject;
                    go.SetActive(!go.activeSelf);
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
            tools.GetChild(oldActive).gameObject.SetActive(false);
            tools.GetChild(activeTool).gameObject.SetActive(true);
        }
    }
}
