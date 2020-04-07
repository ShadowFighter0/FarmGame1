using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour
{
    TimeManager timeScript;
    bool playerIn;

    private void Start()
    {
        timeScript = FindObjectOfType<TimeManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIn)
        {
            //playerScript.MovementActive(false);
            timeScript.NewDay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerIn = false;
    }
}
