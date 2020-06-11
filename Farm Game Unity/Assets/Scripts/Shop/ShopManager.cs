using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    #region Variables 
    public bool onShop = false;
    public GameObject shopPanel;
    public Shop[] shops;
    public TextMeshProUGUI totalPrice;
    public TextMeshProUGUI currentMoney;

    [HideInInspector] public Shop currentShop;

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
        cart = new ShopItem[stockUI.Length];

        totalPrice = shopPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        shops = FindObjectsOfType<Shop>();
        cartView = true;
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
            AmountPanel.Instance.On(50);
    }

    public void ConfirmAmount(int cant)
    {
        if (cartView)
        {
            ShopItem c = cart[pos];

            c.amountSelected = cant;

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

            ShowCart();
        }
        else
        {
            ShopItem s = currentShop.stock[pos];
            int cartPos = Search(s.item.name);

            if (cartPos < 0)
            {
                if (cant > 0)
                {
                    cart[numCart] = s;
                    cart[numCart].amountSelected += cant;

                    numCart++;
                }
            }
            else
            {
                cart[cartPos].amountSelected += cant;
            }
            ShowShop();
        }

        AmountPanel.Instance.Off();
        UpdatePrice();
    }

    private void UpdatePrice()
    {
        totalPrice.text = "Cart value: " + GetCharge().ToString();
    }
    public void UpdatePriceSlider()
    {
        totalPrice.text = "Cart value: " + shops[0].stock[pos].item.price * AmountPanel.Instance.slider.value;
    }

    private int Search(string name)
    {
        for (int i = 0; i < cart.Length; i++)
        {
            if (cart[i] != null && cart[i].item.name.Equals(name))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary> 
    /// Open the shop's panel 
    /// </summary> 
    public void OpenShop(Shop shop)
    {
        onShop = true;
        if (shop != currentShop)
        {
            cart = new ShopItem[stockUI.Length];
            numCart = 0;
        }

        currentShop = shop;
        shopPanel.SetActive(true);
        ShowShop();
        InputManager.instance.ChangeState(InputManager.States.OnUI);
        currentMoney.text = "Your money: " + InventoryController.Instance.GetAmount("Money");
    }

    /// <summary> 
    /// Close the shop's panel 
    /// </summary> 
    public void CloseShop()
    {
        onShop = false;
        shopPanel.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
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
                if (s != null)
                {
                    if (s.item.GetType() == typeof(Seed))
                    {

                    }
                    else if (s.item.GetType() == typeof(Item))
                    {
                        numItems++;
                    }
                }
            }

            if (InventoryController.Instance.itemSpace >= InventoryController.Instance.numItems + numItems)
            {
                InventoryController.Instance.SubstractAmountItem(price, "Money");

                foreach (ShopItem s in cart)
                {
                    if (s != null)
                    {
                        InventoryController.Instance.AddItem(s.item, s.amountSelected);
                        s.amountSelected = 0;
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

    public void ShowShop()
    {
        cartView = false;
        shopPanel.transform.GetChild(3).GetComponent<Scrollbar>().value = 1;

        for (int i = 0; i < currentShop.stock.Length; i++)
        {
            if (currentShop.stock[i].item != null)
            {
                ShopEntry t = stockUI[i];
                t.gameObject.SetActive(true);
                t.Fill(currentShop.stock[i]);
            }
            else
            {
                stockUI[i].gameObject.SetActive(false);
            }
        }
    }
    public void ShowCart()
    {
        cartView = true;
        shopPanel.transform.GetChild(3).GetComponent<Scrollbar>().value = 1;

        for (int i = 0; i < cart.Length; i++)
        {
            if (cart[i] != null && cart[i].item != null)
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
        UpdatePrice();
    }
    #endregion
}