using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem
{
    public Item item;

    public int amountSelected = 0;
    public int amount = 0;

    public SellItem(Item i, int stock)
    {
        item = i;
        amount = stock;
    }
}

public class Sell : MonoBehaviour
{
    // Hacer lo de los hijos pa q se vea chido (GetChild(0).GetChild())
    // Car spawn in GetChild(3)

    SellItem[] stock;
    ShopEntry[] stockUI;

    int numStock = 0;
    int position = 0;

    float timeForNextSell;
    public bool onShopView;
    public bool playerNear;
    public GameObject shopPanel;

    public static Sell Instance;

    private void Start()
    {
        Instance = this;
        stockUI = ShopManager.Instance.stockUI;
        stock = new SellItem [stockUI.Length];
    }

    private void Update()
    {
        if (numStock > 0)
        {
            if (timeForNextSell < 0)
            {
                timeForNextSell = Random.Range(60, 500);
                SellItem();
            }
            else
            {
                timeForNextSell -= Time.deltaTime;
            }
        }
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
        if (onShopView && InventoryController.Instance.inventoryOpen)
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
        AmountPanel.Instance.On(stock[position].amount);
    }

    public void ConfirmReturnItems(int cant)
    {
        Item i = stock[position].item;
        i.amount = cant;
        InventoryController.Instance.AddItem(i);

        stock[position].amount -= cant;
        if(stock[position].amount == 0)
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
            stock[currentPosition].amount += amount;
        }
        else
        {
            stock[numStock] = new SellItem(DataBase.GetItem(id), amount);
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

    public void SellItem()
    {
        //Deploy Car

        int pos = Random.Range(0, numStock);
        SellItem item = stock[pos];
        int cant = Random.Range(0, item.amount);
        MoneyBox.Instance.AddMoney(item.item.price * cant);

        if (cant == item.amount)
        {
            stock[pos] = null;
            ReOrder();
            numStock--;
        }
        else
        {
            item.amount -= cant;
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
