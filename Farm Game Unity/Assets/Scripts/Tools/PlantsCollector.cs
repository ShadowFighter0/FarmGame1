using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsCollector : MonoBehaviour
{
    public Animator anim;
    private bool onAnim;
    void Update()
    {
        GameObject go = RayCastController.instance.GetTarget();
        if (Input.GetKeyDown(InputManager.instance.Interact) && go.CompareTag("Hole"))
        {
            PlantLife script = go.transform.GetComponentInChildren<PlantLife>();
            if (script != null && !onAnim && script.GetGrownUp())
            {
                onAnim = true;
                anim.SetBool("Taking", true);
                StartCoroutine(AnimDelay(go, script));
            }
        }
    }

    IEnumerator AnimDelay(GameObject go, PlantLife script)
    {
        yield return new WaitForSeconds(0.3f);
        onAnim = false;
        anim.SetBool("Taking", false);
        script.AddInventory();
        Destroy(go.transform.GetChild(0).gameObject);
    }
}
