using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    private bool canOpen;
    [SerializeField] GameObject panel = null;
    private Animator anim;
    private void Start()
    {
        anim = panel.GetComponent<Animator>();
        anim.SetBool("Active", false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MovementController.instance.SetMovement(false);
            PlayerFollow.instance.SetMovement(false);
            anim.SetBool("Active", true);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            anim.SetBool("Active", false);
            MovementController.instance.SetMovement(true);
            PlayerFollow.instance.SetMovement(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
            anim.SetBool("Active", false);
        }
    }
}
