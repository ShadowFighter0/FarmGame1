using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public Transform wateringCan;
    public ParticleSystem waterParticles;

    private void Start()
    {
        waterParticles.Stop();
    }
    void Update()
    {
        if (InputManager.state == InputManager.States.Working)
        {
            if (Input.GetKeyDown(InputManager.instance.Click))
            {
                waterParticles.Play();
            }
            if (Input.GetKeyUp(InputManager.instance.Click))
            {
                waterParticles.Stop();
            }
        }
        else
        {
            if(waterParticles.isPlaying)
            {
                waterParticles.Stop();
            }
        }
    }
    private void OnEnable()
    {
        waterParticles.Stop();
    }
}
