﻿using System.Collections;
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
    SellItem[] stock;
    ShopEntry[] stockUI;

    int numStock = 0;
    int position = 0;
    int moneyAmount;
    public Item moneyClass;
    public GameObject UiIcon;

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
        if (playerNear && Input.GetKeyDown(InputManager.instance.Interact))
        {
            OpenShop();
        }
        if (onShopView && InventoryController.Instance.inventoryOpen)
        {
            OpenInventory();
        }
    }

    /// <summary>
    /// Si abres el inventario cuando estas en el Sell se apaga 
    /// </summary>
    private void OpenInventory()
    {
        InputManager.instance.ChangeState(InputManager.States.Idle);
        shopPanel.SetActive(false);
        onShopView = false;
        shopPanel.transform.GetChild(0).gameObject.SetActive(true);    //desoculta Money
        shopPanel.transform.GetChild(4).gameObject.SetActive(true);    //desoculta cartButton
    }

    /// <summary>
    /// Abre o cierra la tienda (UI) si usas la E
    /// </summary>
    private void OpenShop()
    {
        if (!shopPanel.activeSelf)
        {
            InputManager.instance.ChangeState(InputManager.States.OnUI);
            if (InventoryController.Instance.inventoryOpen)
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

    /// <summary>
    /// Añadir dinero a la "caja"
    /// </summary>
    /// <param name="cant"></param>
    public void AddMoney(int cant)
    {
        moneyAmount += cant;
        //TODO q salga bonico esto

        UiIcon.SetActive(true);
        StartCoroutine(UiIconOff());

    }

    /// <summary>
    /// Boton de collect Money 
    /// </summary>
    private void CollectMoney()
    {
        Item aux = moneyClass;
        aux.amount = moneyAmount;
        InventoryController.Instance.AddItem(aux);
        moneyAmount = 0;
    }

    /// <summary>
    /// Presionas un boton de la tienda para devolverlo al inventario (luego entra a ConfirmReturnItems)
    /// </summary>
    /// <param name="pos"></param>
    public void ReturnItems(int pos)
    {
        position = pos;
        AmountPanel.Instance.On(stock[position].amount);
    }

    /// <summary>
    /// desues de usar "Button" usas esta funcion para añadir cuanta cantidad quieres recuperar del item 
    /// </summary>
    /// <param name="cant"></param>
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

    /// <summary>
    /// si eliminas un item de una posicion x todos los objetos por debajo haran x-1
    /// </summary>
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

    /// <summary>
    /// actualiza el panel de la tienda para mostrar los items que estan disponibles para vender
    /// </summary>
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

    /// <summary>
    /// Presionas un boton del inventario para meterlo a la tienda
    /// </summary>
    /// <param name="pos"></param>
    public void Button(int pos)
    {
        position = pos;
    }

    /// <summary>
    /// Añades un Item del inventario al array 
    /// </summary>
    /// <param name="amount"></param>
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

    /// <summary>
    /// buscas la posicion de un objeto concreto en el array (devuelve la posicion o -1)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
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

    /// <summary>
    /// El coche viene y se leva un item
    /// </summary>
    public void SellItem()
    {
        if (numStock > 0)
        {
            int pos = Random.Range(0, numStock);
            SellItem item = stock[pos];
            int cant = Random.Range(item.amount/4, item.amount);

            AddMoney(item.item.price * cant);

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
        
    }

    IEnumerator UiIconOff()
    {
        yield return new WaitForSeconds(3f);
        UiIcon.SetActive(false);
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
