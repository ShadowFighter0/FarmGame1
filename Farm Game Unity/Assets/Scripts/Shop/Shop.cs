using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public Item item;
    public int amountSelected = 0;

    public ShopItem(Item i)
    {
        item = i;
    }
    public ShopItem()
    {
        item = null;
    }
}

public class Shop : MonoBehaviour
{
    public ShopItem[] stock = new ShopItem[21];

    public NpcController owner;
    public bool seeds;
    int num = 0;

    private bool playerNear = false;
    private OutlineController outline;
    private void Start()
    {
        GameEvents.OnSaveInitiated += SaveItems;
        for (int i = 0; i < stock.Length; i++)
        {
            stock[i] = new ShopItem();
        }

        if(SaveLoad.SaveExists("ShopItems"))
        {
            List<string> names = SaveLoad.Load<List<string>>("ShopItems");
            foreach (string s in names)
            {
                Item item = DataBase.GetItem(s);
                AddToStock(item);
            }
        }
        else
        {
            Item seed = DataBase.GetItem("CarrotSeed");
            AddToStock(seed);
        }
        outline = GetComponent<OutlineController>();
    }

    private void Update()
    {
        if (playerNear && Input.GetKey(InputManager.instance.Interact))
        {
            ShopManager.Instance.OpenShop(this);
        }
        if (ShopManager.Instance.shopPanel.activeSelf && Input.GetKey(InputManager.instance.Escape))
        {
            ShopManager.Instance.CloseShop();
        }
    }

    private void SaveItems()
    {
        List<string> names = new List<string>();
        foreach (ShopItem item in stock)
        {
            if(item.item == null)
            {
                break;
            }
            names.Add(item.item.itemName);
        }
        Debug.Log("Saving " + names.Count);

        SaveLoad.Save(names, "ShopItems");
        names.Clear();
    }
    public void AddToStock (Item seed)
    {
        stock[num].item = seed;
        num++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            outline.ShowOutline();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            outline.HideOutline();
        }
    }
}
