﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopController : MonoBehaviour
{
    private bool playerIn = false;
    private bool camActive = false;

    public GameObject workshopCamera;
    private List<UnlockeableItem> unlockableItems = new List<UnlockeableItem>();

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

    private Vector3 oriCamPos;
    private Quaternion oriCamRot;

    public GameObject UIMenu;

    private bool finished = true;

    public Material mat;

    private void Start()
    {
        GameEvents.OnSaveInitiated += SaveItems;
        camPivot = freeCam.GetChild(0);
        newPos = freeCam.position;
        newPivotPos = camPivot.localPosition;

        foreach (UnlockeableItem script in FindObjectsOfType<UnlockeableItem>())
        {
            unlockableItems.Add(script);
        }
        if (SaveLoad.SaveExists("UnlockeableItems"))
        {
            List<bool> bools = SaveLoad.Load<List<bool>>("UnlockeableItems");
            for (int i = 0; i < unlockableItems.Count; i++)
            {
                unlockableItems[i].purchased = bools[i];
                bool b = unlockableItems[i].purchased;
                if (b)
                {
                    unlockableItems[i].gameObject.SetActive(true);
                }
                else
                {
                    unlockableItems[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            ChangeItemsState(false);
        }
    }

    private void Update()
    {
        if(playerIn)
        {
            if (Input.GetKeyDown(InputManager.instance.Interact) && !camActive && finished)
            {
                finished = false;
                UIMenu.SetActive(true);
                InputManager.instance.ChangeState(InputManager.States.OnUI);
            }

            if (Input.GetKeyDown(KeyCode.F1) && !finished)
            {
                if(UIMenu.activeSelf)
                {
                    finished = true;
                    CloseMenu();
                }
                else
                {
                    DisableCamera();
                    ChangeItemsState(false);
                }
            }

            if (camActive)
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
                                    images[i].sprite = item.requirements[i].image;
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

    private void SaveItems()
    {
        List<bool> objects = new List<bool>();
        for (int i = 0; i < unlockableItems.Count; i++)
        {
            objects.Add(unlockableItems[i].purchased);
        }
        SaveLoad.Save(objects, "UnlockeableItems");
    }

    private void CloseMenu()
    {
        UIMenu.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    private void DisableCamera()
    {
        UIMenu.SetActive(true);
        menu.SetActive(false);
        workshopCamera.SetActive(false);
        cam.position = oriCamPos;
        cam.rotation = oriCamRot;
        camActive = false;
    }

    public void ActivateCamera()
    {
        UIMenu.SetActive(false);

        workshopCamera.SetActive(true);
        oriCamPos = cam.position;
        oriCamRot = cam.rotation;
        camActive = true;
        InputManager.instance.ChangeState(InputManager.States.Editing);

        ChangeItemsState(true);
    }

    private void ChangeItemsState(bool b)
    {
        for (int i = 0; i < unlockableItems.Count; i++)
        {
            if(!unlockableItems[i].purchased)
            {
                if(b)
                {
                    unlockableItems[i].SetMaterial(mat);
                }
                unlockableItems[i].gameObject.SetActive(b);
            }
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

            currentItem.GetComponent<UnlockeableItem>().SetOriginalMat();
            currentItem.GetComponent<UnlockeableItem>().purchased = true;

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

            if(menu.activeSelf)
            {
                menu.SetActive(false);
                currentItem = null;
                item = null;
            }
        }

        if (scroll != 0)
        {
            newPivotPos.y -= scroll;
            newPivotPos.y = Mathf.Clamp(newPivotPos.y, 5f, 15f);
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
