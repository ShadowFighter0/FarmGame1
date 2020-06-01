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

    private void Start()
    {
        for (int i = 0; i < stock.Length; i++)
        {
            stock[i] = new ShopItem();
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

    public void AddToStock (Item seed)
    {
        stock[num].item = seed;
        num++;
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
