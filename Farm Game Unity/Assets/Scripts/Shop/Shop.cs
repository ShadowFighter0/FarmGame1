using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public Item item;

    public int stock = 0;
    public int amountSelected = 0;

    public ShopItem(Item i, int amount)
    {
        item = i;
        stock = amount;
    }
}

public class Shop : MonoBehaviour
{
    [Tooltip("No pueden ser mas de 21 items")]public Item[] stockItems;
    public ShopItem[] stock;

    public NpcController owner;

    private bool playerNear = false;

    private void Start()
    {
        stock = new ShopItem[stockItems.Length];

        for(int i = 0; i < stock.Length; i++)
        {
            stock[i] = new ShopItem(stockItems[i], GenerateAmount());
        }
    }

    private void Update()
    {
        if (playerNear && Input.GetKey(InputManager.instance.Interact))
        {
            ShopManager.Instance.OpenShop(this);
        }
        if( ShopManager.Instance.shopPanel.activeSelf && Input.GetKey(InputManager.instance.Escape))
        {
            ShopManager.Instance.CloseShop();
        }
    }

    private int GenerateAmount()
    {
        // q este mejor si eso 
        return Random.Range(3, 10);
    }

    public void NewDay()
    {
        foreach (ShopItem s in stock)
        {
            s.stock = GenerateAmount();
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
