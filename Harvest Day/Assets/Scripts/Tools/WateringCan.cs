using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public Transform wateringCan;
    public ParticleSystem waterParticles;
    private Vector3 oriRot;
    private Vector3 enableRot;
    private Vector3 newRot;
    private bool mousePressed = false;

    private void Start()
    {
        waterParticles.Stop();
        oriRot = transform.localEulerAngles;
        enableRot = transform.localEulerAngles + Vector3.forward * 60.0f;
        newRot = oriRot;
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

        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, newRot, 10 * Time.deltaTime);
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
        newRot = enableRot;
        waterParticles.Play();
    }
    public void Stop()
    {
        newRot = oriRot;
        waterParticles.Stop();
    }
    private void OnEnable()
    {
        waterParticles.Stop();
        InteractButton.Instance.gameObject.SetActive(true);
        InteractButton.Instance.Water(this);
    }
    private void OnDisable()
    {
        InteractButton.Instance.Water(this);
        InteractButton.Instance.gameObject.SetActive(false);
    }
}
