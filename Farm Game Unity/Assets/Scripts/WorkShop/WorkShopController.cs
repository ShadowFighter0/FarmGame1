using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkShopController : MonoBehaviour
{
    private bool playerIn = false;
    private bool camActive = false;

    public GameObject workshopCamera;
    private GameObject[] unlockableItems;
    private MeshRenderer[] mesh;
    private Material[] original;
    public Material mat;

    private void Start()
    {
        unlockableItems = GameObject.FindGameObjectsWithTag("UnlockableItem");
        mesh = new MeshRenderer[unlockableItems.Length];
        original = new Material[unlockableItems.Length];
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            mesh[i] = unlockableItems[i].GetComponent<MeshRenderer>();
            original[i] = mesh[i].material;
            unlockableItems[i].SetActive(false);
        }
    }

    private void Update()
    {
        if(playerIn)
        {
            if (Input.GetKeyDown(InputManager.instance.Interact) && !camActive)
            {
                workshopCamera.SetActive(true);

                camActive = true;
                InputManager.instance.ChangeState(InputManager.States.Editing);

                ChangeItemState(true, mat);
            }
        }
    }

    private void ChangeItemState(bool b, Material m)
    {
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            unlockableItems[i].SetActive(b);
            mesh[i].material = m;
        }
    }

    #region Trigger with player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = false;
        }
    }
    #endregion
}
