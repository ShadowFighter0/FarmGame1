using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedButton : MonoBehaviour
{
    public void Click()
    {
        InputManager.instance.ChangeState(InputManager.States.SelectingSeed);
        RadialMenuController.instance.Open();
    }
}
