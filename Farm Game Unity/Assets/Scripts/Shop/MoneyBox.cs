﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour
{
    int moneyAmount;
    bool inside;
    public Item moneyClass;

    public static MoneyBox Instance;

    private void Awake ()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update ()
    {
        if (inside)
        {
            if(Input.GetKeyDown(InputManager.instance.Interact))
            {
                CollectMoney();
            }
        }
    }

    private void CollectMoney()
    {
        Item aux = moneyClass;
        aux.amount = moneyAmount;
        InventoryController.Instance.AddItem(aux);
        moneyAmount = 0;
    }

    public void AddMoney (int cant)
    {
        moneyAmount += cant;
    }

    private void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inside = true;
        }
    }
    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
        }
    }
}
