﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Path 
    private Transform[] path;
    int index = 0;
    //Driving 
    private bool moving = false;
    private float accel = 8f;
    private float rotateSpeed = 5f;

    private float desireSpeed;
    public float maxVel = 30;
    private float currentSpeed;

    //Shop 
    bool gonnaBuy;

    void Update()
    {
        if (moving)
        {
            float dt = Time.deltaTime;

            // AccelRotation(dt);
            transform.LookAt(path[index]);
            AccelMovement(dt, desireSpeed);
            transform.position = Vector3.MoveTowards(transform.position, path[index].position, currentSpeed * dt);
        }
    }

    public void TurnOn(Transform[] path, bool buy)
    {
        this.path = path;
        index = 0;
        transform.position = path[index].position;
        transform.eulerAngles = path[index].localEulerAngles;
        index++;
        gonnaBuy = buy;
        moving = true;
        desireSpeed = maxVel;
    }

    private void TurnOff()
    {
        moving = false;
        gonnaBuy = false;
        index = 0;
        transform.position = path[index].position;
        desireSpeed = 0f;
    }

    private void AccelMovement(float dt, float desireSpeed)
    {
        float targetZSpeed = desireSpeed;
        float velZOffset = targetZSpeed - currentSpeed;
        velZOffset = Mathf.Clamp(velZOffset, -accel * dt, accel * dt);
        currentSpeed += velZOffset;
    }

    private void AccelRotation(float dt)
    {
        Vector3 desireAngle = path[index].position - transform.position;

        float angleOffset = Vector3.Angle(transform.forward, desireAngle);

        angleOffset = Mathf.Clamp(angleOffset, -rotateSpeed * dt, rotateSpeed * dt);

        if (transform.position.z < path[index].position.z)
            angleOffset *= -1;

        transform.Rotate(transform.up * angleOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            if (index == path.Length - 1)
            {
                TurnOff();
                CarManager.Instance.EndRoute(this);
                return;
            }
            else if (index == 3)
            {
                if (gonnaBuy)
                {
                    desireSpeed = maxVel / 3;
                }
                else
                {
                    index += 3;
                }
            }
            else if (index == 5)
            {
                Sell.Instance.SellItem();
                desireSpeed = 0;
                StartCoroutine(Continue());
            }
            index++;
        }
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(2f);
        desireSpeed = maxVel;
    }
}