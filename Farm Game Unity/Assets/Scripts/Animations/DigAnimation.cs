using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigAnimation : StateMachineBehaviour
{
    private bool digged = false;
    private bool secondSound = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.ChangeState(InputManager.States.Working);
        MovementController.instance.StopMovement();
        digged = false;
        secondSound = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentFrame = stateInfo.normalizedTime;

        if (currentFrame >= .3f && !digged)
        {
            digged = true;
            FindObjectOfType<Hoe>().CreateHole();
            AudioManager.PlaySound(DataBase.GetAudioClip("Dig"));
        }
        if(currentFrame >= .75f && !secondSound)
        {
            secondSound = true;
            AudioManager.PlaySound(DataBase.GetAudioClip("Dig2"));
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
}
