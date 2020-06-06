using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Path
    Transform[] path;
    int index = 0;
    //Driving
    private bool moving = false;
    public int speed = 5;
    float currentSpeed;
    //Shop
    bool gonnaBuy;

    void Update()
    {
        if(moving)
        {
            //mirar hacia donde tiene q mirar progresivamente
            //acelerar y frenar poco poco 

            transform.position = Vector3.MoveTowards(transform.position, path[index].position, 5);

            if (gonnaBuy)
            {
                Sell.Instance.SellItem();
            }
            if (index == path.Length - 1)
            {
                TurnOff();
            }
        }
        
    }

    public void TurnOn (Transform[] path, bool buy)
    {
        transform.position = path[0].position;
        transform.forward = path[0].forward;
        gonnaBuy = buy;
        currentSpeed = 5;
    }

    private void TurnOff ()
    {
        gonnaBuy = false;
        index = 0;
        transform.position = path[index].position;
        currentSpeed = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            index++;
            if (index == 4 && !gonnaBuy)
            {

                index += 2;
            }
        }
    }
}