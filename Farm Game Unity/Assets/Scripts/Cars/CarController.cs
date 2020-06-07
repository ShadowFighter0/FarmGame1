using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Path
    private Transform[] path;
    int index = 0;
    //Driving
    private bool moving = false;
    private float accel = 7f; 

    public float desireSpeed;
    public float maxVel = 5;
    private float currentSpeed;

    //Shop
    bool gonnaBuy;

    void Update()
    {
        if (moving)
        {
            float dt = Time.deltaTime;
            transform.LookAt(path[index]);
            GetOffset(dt, desireSpeed);
            transform.position = Vector3.MoveTowards(transform.position, path[index].position, currentSpeed*dt);
        }
    }

    public void TurnOn (Transform[] path, bool buy)
    {
        this.path = path;
        index = 0;
        transform.position = path[0].position;
        transform.forward = path[0].forward;
        gonnaBuy = buy;
        moving = true;
        desireSpeed = maxVel;
    }

    private void TurnOff ()
    {
        moving = false;
        gonnaBuy = false;
        index = 0;
        transform.position = path[index].position;
        desireSpeed = 0f;
    }


    private float GetOffset(float dt,  float desireSpeed)
    {
        float targetZSpeed = desireSpeed;
        float velZOffset = targetZSpeed - currentSpeed;
        velZOffset = Mathf.Clamp(velZOffset, -accel * dt, accel * dt);
        currentSpeed += velZOffset;
        return currentSpeed;
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(5f);
        desireSpeed = maxVel;
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
                    desireSpeed = 3*maxVel / 4;
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
}