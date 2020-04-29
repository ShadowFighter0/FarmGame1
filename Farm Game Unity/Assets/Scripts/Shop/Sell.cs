using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sell : MonoBehaviour
{
    ShopItem[] stock;
    ShopEntry[] stockUI;

    int numStock = 0;
    int position = 0;

    public bool onSell;
    public bool playerNear;
    public GameObject shopPanel;

    public static Sell Instance;


    private void Awake()
    {
        Instance = this;
        stockUI = ShopManager.Instance.stockUI;
    }

    private void Update()
    {
        if (playerNear && Input.GetKey(InputManager.instance.Interact))
        {
            if (!shopPanel.activeSelf)
            {
                shopPanel.SetActive(true);
                ShowStock();
                onSell = true;
                //mostrar lo del panel shop pero F
            }
            else
            {
                shopPanel.SetActive(false);
                onSell = false;
                //cerrar el panel shop
            }

        }
    }

    public void ReturnItems(int position)
    {
        AmountPanel.Instance.gameObject.SetActive(true);
        AmountPanel.Instance.On(stock[position].stock);
    }

    private void ShowStock()
    {
        for (int i = 0; i < stockUI.Length; i++)
        {
            if (stock[i] != null)
            {
                stockUI[i].gameObject.SetActive(true);
                stockUI[i].Fill(stock[i]);
            }
            else
            {
                stockUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void Button(int pos)
    {
        position = pos;
    }

    public void AddItem(int amount)
    {
        int currentPosition = SearchStock(InventoryController.Instance.GetID(position));
        //Añadir a stock
        if (currentPosition < 0)
        {
            string id = InventoryController.Instance.GetID(position);
            stock[numStock].item = DataBase.GetItem(id);
            stock[numStock].stock = amount;
            numStock++;

            InventoryController.Instance.SubstractAmountItem(amount, id);
        }
        else
        {
            stock[position].stock += amount;
        }
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
        if (shopPanel.activeInHierarchy)
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
            playerNear = false;

    }
}
