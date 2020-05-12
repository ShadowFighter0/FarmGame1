using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public Transform wateringCan;
    public ParticleSystem waterParticles;

    private bool mousePressed = false;

    private void Start()
    {
        waterParticles.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.Click) && InputManager.state == InputManager.States.Idle)
        {
            mousePressed = true;
            StartCoroutine(MouseHold());
        }
        if(Input.GetKeyUp(InputManager.instance.Click) && mousePressed)
        {
            mousePressed = false;
            InputManager.instance.playerAnim.SetTrigger("Watering");
        }
    }
    IEnumerator MouseHold()
    {
        yield return new WaitForSeconds(0.1f);
        if(mousePressed)
        {
            mousePressed = false;
            InputManager.instance.playerAnim.SetTrigger("MultiWatering");
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
