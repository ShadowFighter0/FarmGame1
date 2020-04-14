using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopCameraController : MonoBehaviour
{
    public Transform cam;
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
    private void Start()
    {
        camPivot = transform.GetChild(0);
        newPos = transform.position;
        newPivotPos = camPivot.localPosition;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        Movement(dt);

        if(Input.GetKeyDown(InputManager.instance.Click))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.CompareTag("UnlockableItem"))
                {
                    menu.SetActive(true);

                    item = hit.transform.GetComponent<UnlockeableItem>();
                    if(item != null)
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

        if(scroll != 0)
        {
            newPivotPos.y -= scroll;
            //newPivotPos.y = Mathf.Clamp(newPos.y, 5f, 15f);
        }

        cam.position = camPivot.position;
        cam.rotation = camPivot.rotation;

        transform.position = Vector3.Lerp(transform.position, newPos, 10 * dt);
        camPivot.localPosition = Vector3.Lerp(camPivot.localPosition, newPivotPos, 10 * dt);
    }
}
