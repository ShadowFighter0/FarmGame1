using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.Interact))
        {
            //animation que no hay
            GameObject plant = RayCastController.instance.GetPlant();
            PlantLife script = plant.GetComponent<PlantLife>();
            script.GrownUp();
            //unas particulitas to guapas
        }
    }
}
