﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopController : MonoBehaviour
{
    private bool playerIn = false;
    private bool camActive = false;

    public GameObject workshopCamera;
    private GameObject[] unlockableItems;
    private MeshRenderer[] mesh;
    private Material[] original;
    public Material mat;

    public Transform cam;
    public Transform freeCam;
    private Transform camPivot;
    
    private Vector3 newPos;
    private Vector3 newPivotPos;
    public float speed;

    private RaycastHit hit;
    private Ray ray;

    public GameObject menu;
    public Text itemName;
    public Text description;
    public Image[] images;
    public Text[] amounts;

    private UnlockeableItem item;
    private GameObject currentItem;

    private void Start()
    {
        camPivot = freeCam.GetChild(0);
        newPos = freeCam.position;
        newPivotPos = camPivot.localPosition;

        unlockableItems = GameObject.FindGameObjectsWithTag("UnlockableItem");
        mesh = new MeshRenderer[unlockableItems.Length];
        original = new Material[unlockableItems.Length];
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            mesh[i] = unlockableItems[i].GetComponent<MeshRenderer>();
            Material[] mats = new Material[2];
            original[i] = mesh[i].material;
            mats[0] = original[i];
            mats[1] = mat;
            mesh[i].materials = mats;
            
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

                ChangeItemsState(true, 0);
            }

            if(camActive)
            {
                float dt = Time.deltaTime;

                Movement(dt);

                if (Input.GetKeyDown(InputManager.instance.Click))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.CompareTag("UnlockableItem"))
                        {
                            menu.SetActive(true);
                            currentItem = hit.transform.gameObject;
                            item = hit.transform.GetComponent<UnlockeableItem>();
                            if (item != null)
                            {
                                itemName.text = item.itemName;
                                description.text = item.description;

                                for (int i = 0; i < item.requirements.Length; i++)
                                {
                                    Sprite sprite = Resources.Load<Sprite>("Sprites/" + item.requirements[i].imagePath);
                                    images[i].sprite = sprite;
                                    amounts[i].text = item.amounts[i].ToString();
                                }
                            }

                            newPos = hit.transform.position;
                        }
                    }
                }
            }
        }
    }

    private void ChangeItemsState(bool b, int m)
    {
        for (int i = 0; i < unlockableItems.Length; i++)
        {
            unlockableItems[i].SetActive(b);
            mesh[i].material = mesh[i].materials[m];
        }
    }

    public void BuyItem()
    {
        if (item != null)
        {
            for (int i = 0; i < item.requirements.Length; i++)
            {
                int amount = InventoryController.Instance.GetAmount(item.requirements[i].itemName);
                if (amount <= 0)
                {
                    Debug.Log("You dont have enought materials!");
                    return;
                }
            }

            for (int i = 0; i < item.requirements.Length; i++)
            {
                InventoryController.Instance.SubstractAmountItem(item.amounts[i], item.requirements[i].itemName);
            }
            Debug.Log(item.itemName + " purchased!");
            menu.SetActive(false);

            MeshRenderer m = currentItem.GetComponent<MeshRenderer>();
            m.material = m.materials[1];

            currentItem.tag = "Untagged";
            currentItem = null;
            item = null;
        }
    }
    private void Movement(float dt)
    {
        Vector3 input;
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");
        input.y = 0;

        int scroll = (int)Input.mouseScrollDelta.y;

        if (input.sqrMagnitude > Mathf.Epsilon)
        {
            newPos += input * speed * dt;
        }

        if (scroll != 0)
        {
            newPivotPos.y -= scroll;
            //newPivotPos.y = Mathf.Clamp(newPos.y, 5f, 15f);
        }

        cam.position = camPivot.position;
        cam.rotation = camPivot.rotation;

        freeCam.position = Vector3.Lerp(freeCam.position, newPos, 10 * dt);
        camPivot.localPosition = Vector3.Lerp(camPivot.localPosition, newPivotPos, 10 * dt);
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
