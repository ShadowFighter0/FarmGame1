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
}

public class Shop : MonoBehaviour
{
    private Item[] stockItems;
    public ShopItem[] stock;

    public NpcController owner;
    public bool seeds;

    private bool playerNear = false;

    private void Start()
    {
        if(seeds)
        {
            stockItems = Resources.LoadAll<Seed>("Data/Items/Seeds");
        }
        else
        {
            stockItems = Resources.LoadAll<Item>("Data/Items/Items");
        }

        stock = new ShopItem[stockItems.Length];

        for (int i = 0; i < stock.Length; i++)
        {
            stock[i] = new ShopItem(stockItems[i]);
        }
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
}
