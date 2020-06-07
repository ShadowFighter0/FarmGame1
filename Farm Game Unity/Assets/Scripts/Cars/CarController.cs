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
    public int speed = 1;
    //Shop
    bool gonnaBuy;

    void Update()
    {
        if (moving)
        {
            transform.LookAt(path[index]);
            transform.position = Vector3.MoveTowards(transform.position, path[index].position, speed);

            if (gonnaBuy)
            {
                Sell.Instance.SellItem();
            }
        }
    }

    public void TurnOn (Transform[] path, bool buy)
    {
        this.path = path;
        transform.position = path[0].position;
        transform.forward = path[0].forward;
        gonnaBuy = buy;
        moving = true;
        speed = 1;
    }

    private void TurnOff ()
    {
        moving = false;
        gonnaBuy = false;
        index = 0;
        transform.position = path[index].position;
        speed = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            if (index == path.Length - 1)
            {
                TurnOff();
                CarManager.Instance.EndRoute(this);
            }
            else
            {
                index++;

                if (index == 4 && !gonnaBuy)
                {
                    index += 3;
                }
            }
        }
    }
}