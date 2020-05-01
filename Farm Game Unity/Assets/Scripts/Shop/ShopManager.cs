﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;

    #region Variables
    public bool onShop = false;
    public GameObject shopPanel;
    public Shop[] shops;
    public Text totalPrice;

    public Shop currentShop;

    public ShopEntry[] stockUI; // VisualEntrys

    private ShopItem[] cart; //Items that will be added to your inventory
    [HideInInspector] public bool cartView;

    private int numCart = 0;
    int pos;
    #endregion

    #region Functions

    private void Awake()
    {
        Instance = this;
        GameEvents.OnNewDay += NewDay;
        cart = new ShopItem[stockUI.Length];

        totalPrice = shopPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>();
    }

    /// <summary>
    /// Button click select an item
    /// </summary>
    /// <param name="pos" ></param>
    public void Select(int pos)
    {
        this.pos = pos;
        AmountPanel.Instance.gameObject.SetActive(true);
        if (cartView)
            AmountPanel.Instance.On(cart[pos].amountSelected);
        else
            AmountPanel.Instance.On(currentShop.stock[pos].stock);
    }

    public void ConfirmAmount(int cant)
    {
        if (cartView)
        {
            ShopItem c = cart[pos];
            ShopItem s = currentShop.stock[SearchStock(c.item.name)];

            c.amountSelected -= cant;
            s.stock += cant;

            if (c.amountSelected <= 0)
            {
                cart[pos] = null;
                numCart--;

                for (int i = pos; i < cart.Length; i++)
                {
                    if (i != cart.Length - 1)
                    {
                        cart[i] = cart[i + 1];
                    }
                    else
                    {
                        cart[i] = null;
                    }
                }
            }

            cartView = false;
            CartView();
        }
        else
        {
            ShopItem s = currentShop.stock[pos];

            s.amountSelected += cant;
            s.stock -= cant;

            if (cant > 0)
            {
                cart[numCart] = s;
                numCart++;
            }
            cartView = true;
            CartView();
        }

        AmountPanel.Instance.Off();
        totalPrice.text = GetCharge().ToString();
    }

    private int SearchStock(string name)
    {
        for (int i = 0; i < currentShop.stock.Length; i++)
        {
            if (currentShop.stock[i].item.name == name)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Open the shop's panel and Time.scaleTime = 0;
    /// </summary>
    public void OpenShop(Shop shop)
    {
        onShop = true;
        if (shop != currentShop)
        {
            for (int i = 0; i < cart.Length; i++)
            {
                if (cart[i] != null)
                {
                    currentShop.stock[SearchStock(cart[i].item.name)].stock += cart[i].amountSelected;
                    cart[i].amountSelected = 0;
                }
            }
            cart = new ShopItem[stockUI.Length];
            numCart = 0;
        }

        currentShop = shop;
        cartView = true;
        shopPanel.SetActive(true);
        CartView();
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }

    /// <summary>
    /// Close the shop's panel and Time.scaleTime = 1
    /// </summary>
    public void CloseShop()
    {
        //
        onShop = false;
        shopPanel.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    /// <summary>
    /// New Day
    /// </summary>
    /// 
    public void NewDay()
    {
        foreach (Shop s in shops)
        {
            s.NewDay();
        }
    }

    /// <summary>
    ///Once you have selected all the items you wanna buy get the total price and give the items 
    /// </summary>
    private int GetCharge() // Comprobar si funciona q ha saber
    {
        int price = 0;
        for (int i = 0; i < cart.Length && cart[i] != null; i++)
        {
            price += cart[i].item.price * cart[i].amountSelected;
        }
        return price;
    }

    private void NotEnoughtMoney() //Cambiar por dialogo 
    {
        DialogueSystem.instance.UpdateDialogue("You don't have enought money to buy all");
    }
    private void NotEnoughtSpace() //Cambiar por dialogo 
    {
        DialogueSystem.instance.UpdateDialogue("You don't have enought space to store all");
    }

    public void GiveItems()   // comprobar si funciona q a saber 
    {
        int numItems = 0;
        int price = GetCharge();

        if (InventoryController.Instance.GetAmount("Money") >= price)
        {
            foreach (ShopItem s in cart)
            {
                if (s != null && s.item.GetType() == typeof(Item))
                {
                    numItems++;
                }
            }

            if (InventoryController.Instance.itemSpace >= InventoryController.Instance.numItems + numItems)
            {
                InventoryController.Instance.SubstractAmountItem(price, "Money");

                foreach (ShopItem s in cart)
                {
                    if (s != null)
                    {
                        s.item.amount = s.amountSelected;
                        InventoryController.Instance.AddItem(s.item);
                    }

                }
                cart = new ShopItem[stockUI.Length];
                numCart = 0;
            }
            else
            {
                NotEnoughtSpace();
            }
        }
        else
        {
            NotEnoughtMoney();
        }
    }

    public void CartView()
    {
        cartView = !cartView;
        shopPanel.transform.GetChild(3).GetComponent<Scrollbar>().value = 1;

        if (cartView)
        {
            for (int i = 0; i < cart.Length; i++)
            {
                if (cart[i] != null)
                {
                    ShopEntry t = stockUI[i];
                    t.Fill(cart[i]);
                    t.gameObject.SetActive(true);
                }
                else
                {
                    stockUI[i].gameObject.SetActive(false);
                }
            }
            totalPrice.text = GetCharge().ToString();
        }
        else
        {
            int i = 0;
            for (; i < currentShop.stock.Length; i++)
            {
                ShopEntry t = stockUI[i];
                t.gameObject.SetActive(true);
                t.Fill(currentShop.stock[i]);
            }
            for (; i < stockUI.Length; i++)
            {
                stockUI[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
