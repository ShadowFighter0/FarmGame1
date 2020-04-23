using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public Transform wateringCan;
    public ParticleSystem waterParticles;
    public Animator anim;

    private void Start()
    {
        waterParticles.Stop();
    }

    public void SetAnimator(Animator an)
    {
        anim = an;
    }
    void Update()
    {
        if(InputManager.state != InputManager.States.OnUI)
        {
            if (Input.GetKeyDown(InputManager.instance.Click))
            {
                anim.SetTrigger("Watering");
                InputManager.instance.ChangeState(InputManager.States.Working);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Watering"))
            {
                if (!waterParticles.isPlaying)
                {
                    waterParticles.Play();
                }
            }
            else if (!waterParticles.isStopped)
            {
                waterParticles.Stop();
                InputManager.instance.ChangeState(InputManager.States.Idle);
            }
        }
    }
    private void OnEnable()
    {
        waterParticles.Stop();
    }
}
