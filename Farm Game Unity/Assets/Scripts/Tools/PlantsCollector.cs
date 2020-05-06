using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsCollector : MonoBehaviour
{
    private bool onAnim;

    public static PlantsCollector instance;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.Interact))
        {
            GameObject go = RayCastController.instance.GetTarget();
            if (go.CompareTag("Hole"))
            {
                PlantLife script = go.transform.GetComponentInChildren<PlantLife>();
                if (script != null && !onAnim && script.GetGrownUp())
                {
                    onAnim = true;
                    InputManager.instance.playerAnim.SetTrigger("Taking");
                    StartCoroutine(AnimDelay(go, script));
                }
            }
        }
    }

    IEnumerator AnimDelay(GameObject go, PlantLife script)
    {
        yield return new WaitForSeconds(0.3f);
        onAnim = false;
        script.AddInventory();
        Destroy(go.transform.GetChild(0).gameObject);
    }
}
