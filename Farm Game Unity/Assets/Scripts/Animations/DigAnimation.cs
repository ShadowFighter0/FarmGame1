using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigAnimation : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.ChangeState(InputManager.States.Working);
        MovementController.instance.StopMovement();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
}
