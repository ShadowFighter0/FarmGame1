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

    private void Awake()
    {
        GameEvents.OnAnimatorSelected += SetAnimator;
    }
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
            if (Input.GetKeyDown(InputManager.instance.Click) && InputManager.state != InputManager.States.Working)
            {
                anim.SetTrigger("Watering");
                //transform.eulerAngles = oriRot + Vector3.forward * 60;
            }
        }
    }

    public void Play()
    {
        waterParticles.Play();
    }
    public void Stop()
    {
        waterParticles.Stop();
    }
    private void OnEnable()
    {
        waterParticles.Stop();
    }
}
