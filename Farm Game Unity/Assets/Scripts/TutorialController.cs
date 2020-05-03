using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private int state = -1;

    public static TutorialController instance;
    private void Awake()
    {
        instance = this;
    }

    public void NextState()
    {
        state++;
        UpdateStates();
    }

    private void UpdateStates()
    {
        switch (state)
        {
            case 0:
                NpcManager.instance.StartForcedDialogue("Maria", "Hi! I'm Maria. In the mailbox next to me you will find your first order!\nCome back to me when you have finished");
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
