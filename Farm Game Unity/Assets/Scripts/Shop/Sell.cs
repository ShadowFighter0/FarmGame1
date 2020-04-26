using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sell : MonoBehaviour
{
    ShopItem[] stock;
    ShopEntry[] stockUI;

    public bool sellOpen;
    public bool playerNear;
    public GameObject shopPanel;
    
    public static Sell Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        stockUI = ShopManager.Instance.stockUI;
    }

    private void Update()
    {
    }

    public void AddItem(int pos)
    {

    }
    public void ConfirmAmount()
    {

    }
    private int SearchStock(string name)
    {
        for (int i = 0; i < stock.Length; i++)
        {
            if (stock[i].item.name == name)
            {
                return i;
            }
        }
        return -1;
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    public void ChangeView()
    {
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
        if(shopPanel.activeInHierarchy)
        {
            for (int i = 0; i < stock.Length; i++)
            {
                if (stock[i] != null)
                {
                    ShopEntry t = stockUI[i];
                    t.Fill(stock[i]);
                    t.gameObject.SetActive(true);
                }
                else
                {
                    ShopManager.Instance.stockUI[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void SellItem(int pos)
    {
        //TODO random time and random item
        //TODO show cars
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            
        }
           
    }
}
