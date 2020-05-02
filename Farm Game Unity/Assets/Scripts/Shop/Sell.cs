using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sell : MonoBehaviour
{
    ShopItem[] stock;
    ShopEntry[] stockUI;

    int numStock = 0;
    int position = 0;

    public bool onShopView;
    public bool playerNear;
    public GameObject shopPanel;

    public static Sell Instance;

    private void Start()
    {
        Instance = this;
        stockUI = ShopManager.Instance.stockUI;
        stock = new ShopItem[stockUI.Length];
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(InputManager.instance.Interact))
        {
            if (!shopPanel.activeSelf)
            {
                InputManager.instance.ChangeState(InputManager.States.OnUI);
                if(InventoryController.Instance.inventoryOpen)
                    InventoryController.Instance.CloseMenu();
                shopPanel.transform.GetChild(0).gameObject.SetActive(false);    //oculta Money
                shopPanel.transform.GetChild(4).gameObject.SetActive(false);    //oculta cartButton

                shopPanel.SetActive(true);
                ShowStock();
                onShopView = true;
            }
            else
            {
                InputManager.instance.ChangeState(InputManager.States.Idle);
                shopPanel.SetActive(false);
                onShopView = false;
                shopPanel.transform.GetChild(0).gameObject.SetActive(true);    //desoculta Money
                shopPanel.transform.GetChild(4).gameObject.SetActive(true);    //desoculta cartButton
            }
        }
        if(onShopView && InventoryController.Instance.inventoryOpen)
        {
            InputManager.instance.ChangeState(InputManager.States.Idle);
            shopPanel.SetActive(false);
            onShopView = false;
            shopPanel.transform.GetChild(0).gameObject.SetActive(true);    //desoculta Money
            shopPanel.transform.GetChild(4).gameObject.SetActive(true);    //desoculta cartButton
        }
    }

    public void ReturnItems(int pos)
    {
        position = pos;
        AmountPanel.Instance.On(stock[position].stock);
    }

    public void ConfirmReturnItems(int cant)
    {
        Item i = stock[position].item;
        i.amount = cant;
        InventoryController.Instance.AddItem(i);

        stock[position].stock -= cant;
        if(stock[position].stock == 0)
        {
            ReOrder();
            numStock--;
        }
        ShowStock();
    }
    private void ReOrder()
    {
        for(int i = position; i < stock.Length && stock[i] != null; i++)
        {
            if(i != stock.Length - 1 )
            {
                stock[i] = stock[i + 1];
            }
            else
            {
                stock[i] = null;
            }
        }
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
        string id = InventoryController.Instance.GetID(position);
        //Añadir a stock
        if (currentPosition >= 0)
        {
            stock[currentPosition].stock += amount;
        }
        else
        {
            stock[numStock] = new ShopItem(DataBase.GetItem(id), amount);
            numStock++;
        }
        InventoryController.Instance.SubstractAmountItem(amount, id);
    }

    private int SearchStock(string name)
    {
        for (int i = 0; i < stock.Length; i++)
        {
            if (stock[i] != null && stock[i].item.name == name)
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
