using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToolAnimation : StateMachineBehaviour
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
        
        if(currentFrame >= .5f && !toolChanged)
        {
            toolChanged = true;
            InputManager.instance.ChangeVisualTool();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.SetCanChangeTool(true);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
