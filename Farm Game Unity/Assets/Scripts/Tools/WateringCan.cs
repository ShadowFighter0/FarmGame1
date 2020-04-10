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
            if (Input.GetMouseButtonDown(0))
            {
                waterParticles.Play();
            }
            if (Input.GetMouseButtonUp(0))
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
