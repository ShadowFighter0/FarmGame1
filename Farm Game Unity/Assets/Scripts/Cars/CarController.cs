using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Transform[] path;
    int index = 0;
    public int speed = 5;
    bool gonnaBuy;
    float currentSpeed;

    void Update()
    {
        //mirar hacia donde tiene q mirar progresivamente
        //acelerar y frenar poco poco 

        transform.LookAt(path[index]);
        transform.position += Vector3.forward * currentSpeed * Time.deltaTime;
        
        
        if (gonnaBuy)
            Sell.Instance.SellItem();

        if (index == path.Length-1)
        {
            TurnOff();
        }
    }

    public void TurnOn (bool buy)
    {
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