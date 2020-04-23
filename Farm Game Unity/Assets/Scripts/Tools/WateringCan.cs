using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public Transform wateringCan;
    public ParticleSystem waterParticles;
    public Animator anim;

    private Vector3 oriRot;
    private Vector3 newRot;

    private void Start()
    {
        waterParticles.Stop();
        oriRot = transform.eulerAngles;
        newRot = oriRot;
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
                //transform.eulerAngles = oriRot + Vector3.forward * 60;
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
                //transform.eulerAngles = oriRot;
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
