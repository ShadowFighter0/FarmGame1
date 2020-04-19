﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public Item item;

    public int stock = 0;
    public int amountSelected = 0;

    bool isSelected = false;

    public ShopItem(Item i, int amount)
    {
        item = i;
        stock = amount;
    }

    public void Select()
    {
        isSelected = !isSelected;
    }
    public void CloseShop()
    {
        isSelected = true;
    }
}

public class Shop : MonoBehaviour
{
    public Item[] stockItems;
    public ShopItem[] stock;

    private bool playerNear = false;

    private void Start()
    {
        stock = new ShopItem[stockItems.Length];

        for(int i = 0; i < stock.Length; i++)
        {
            stock[i] = new ShopItem(stockItems[i], GenerateAmount());
            Debug.Log(stock[i].item);
        }
    }

    private void Update()
    {
        if(playerNear && Input.GetKey(KeyCode.E))
        {
            ShopManager.Instance.OpenShop(this);
        }
    }

    private int GenerateAmount()
    {
        // q este mejor si eso 
        return Random.Range(3, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }

    public void NewDay()
    {
        foreach(ShopItem s in stock)
        {
            s.stock = GenerateAmount();
        }
    }
}
