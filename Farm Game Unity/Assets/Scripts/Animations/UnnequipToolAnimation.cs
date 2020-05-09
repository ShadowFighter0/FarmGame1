using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnnequipToolAnimation : StateMachineBehaviour
{
    private bool toolChanged = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.SetCanChangeTool(false);
        InputManager.instance.ChangeState(InputManager.States.Working);
        toolChanged = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentFrame = stateInfo.normalizedTime;

        if (currentFrame >= .5f && !toolChanged)
        {
            toolChanged = true;
            InputManager.instance.Unnequip();
            AudioManager.PlaySound(DataBase.SearchClip("ChangeTool"));
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.SetCanChangeTool(true);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
}
