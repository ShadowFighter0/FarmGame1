using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public Transform wateringCan;
    public ParticleSystem waterParticles;

    private Vector3 oriRot;
    private Vector3 newRot;

    private void Start()
    {
        waterParticles.Stop();
        oriRot = transform.eulerAngles;
        newRot = oriRot;
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.Click) && InputManager.state == InputManager.States.Idle)
        {
            InputManager.instance.playerAnim.SetTrigger("Watering");
            //transform.eulerAngles = oriRot + Vector3.forward * 60;
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
