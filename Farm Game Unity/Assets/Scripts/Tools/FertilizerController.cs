using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.Interact) && InputManager.state == InputManager.States.Idle)
        {
            GameObject go = RayCastController.instance.GetTarget();
            if (go.CompareTag("Hole"))
            {
                PlantLife script = go.transform.GetComponentInChildren<PlantLife>();
                if (script != null)
                {
                    script.GrownUp();
                }
            }
        }
    }
}
