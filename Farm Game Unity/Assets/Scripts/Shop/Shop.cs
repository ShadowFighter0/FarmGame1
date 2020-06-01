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
    int num = 0;

    private bool playerNear = false;

    private void Start()
    {
        stock = new ShopItem[21];
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

    public void AddToStock (Seed seed)
    {
        stock[num] = new ShopItem(seed);
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
