using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsCollector : MonoBehaviour
{
    void Update()
    {
        if (InputManager.instance.editing)
        {
            GameObject go = RayCastController.instance.GetTarget();
            if (Input.GetKeyDown(KeyCode.E) && go.CompareTag("Hole"))
            {
                PlantLife script = go.transform.GetComponentInChildren<PlantLife>();
                if (script.GetGrownUp())
                {
                    script.AddInventory();
                    Destroy(go.transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
