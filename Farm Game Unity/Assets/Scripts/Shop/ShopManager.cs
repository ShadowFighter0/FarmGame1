using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    public GameObject shopPanel;
    public GameObject amountPanel;
    public GameObject errorPanel;
    public Shop[] shops;
    public Text totalPrice;

    private Slider slider;
    private InputField input;

    Shop currentShop;

    public ShopEntry[] stockUI; // VisualEntrys
    
    private ShopItem[] cart; //Items that will be added to your inventory

    private bool confirmSell = false; //Button that confirm the sell or buy 
    public bool cartView;

    private int numCart = 0;
    int pos;
    #endregion


    #region Functions

    private void Start()
    {
        cart = new ShopItem[stockUI.Length];

        totalPrice = shopPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        slider = amountPanel.transform.GetChild(3).GetComponent<Slider>();
        input = amountPanel.transform.GetChild(4).GetComponent<InputField>();
    }

    /// <summary>
    /// Button click select an item
    /// </summary>
    /// <param name="pos"></param>
    public void Select(int pos)
    {
        this.pos = pos;
        slider.value = 0;
        SliderValueChange();

        if(cartView)
            slider.maxValue = cart[pos].amountSelected;
        else
            slider.maxValue = currentShop.stock[pos].stock;
        amountPanel.SetActive(true);
    }

    public void ConfirmAmount ()
    {
        ShopItem s = currentShop.stock[pos];
        ShopItem c = cart[pos];

        if (cartView)
        {
            c.amountSelected -= (int)slider.value;
            s.stock += (int)slider.value;

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
            s.amountSelected += (int)slider.value;
            s.stock -= (int)slider.value;

            if ((int)slider.value > 0)
            {
                cart[numCart] = s;
                numCart++;
            }
            cartView = true;
            CartView();
        }

        amountPanel.SetActive(false);
    }

    public void SliderValueChange()
    {
        input.text = slider.value.ToString();
    }

    public void InputValueChange()
    {
        slider.value = int.Parse(input.text);
    }

    /// <summary>
    /// Open the shop's panel and Time.scaleTime = 0;
    /// </summary>
    public void OpenShop(Shop shop)
    {
        if(shop != currentShop)
        {
            cart = new ShopItem[stockUI.Length];
        }

        currentShop = shop;
        cartView = true;
        shopPanel.SetActive(true);
        CartView();
        
        Time.timeScale = 0;
        InputManager.instance.ChangeState(InputManager.States.OnUI);
        
    }

    /// <summary>
    /// Close the shop's panel and Time.scaleTime = 1
    /// </summary>
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;
        InputManager.instance.ChangeState(InputManager.States.Idle);

        for (int i  = 0; i < stockUI.Length; i++)
        {
            stockUI[i].CloseShop();
            if(i < currentShop.stock.Length - 1)
            {
                currentShop.stock[i].CloseShop();
            }
        }
        confirmSell = false;
    }

    /// <summary>
    /// New Day
    /// </summary>
    /// 
    public void ReFillShop()
    {
        //pillar items de Assets
        foreach(Shop s in shops)
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
        for(int i = 0; i < cart.Length && cart[i]!=null; i++)
        {
            price += cart[i].item.price * cart[i].amountSelected;
        }
        return price;
    }

    private void NotEnoughtMoney()
    {
        errorPanel.SetActive(true);
        errorPanel.transform.GetChild(1).GetComponent<Text>().text = "You don't have enought money to buy all";
    }
    private void NotEnoughtSpace()
    {
        errorPanel.SetActive(true);
        errorPanel.transform.GetChild(1).GetComponent<Text>().text = "You don't have enought space to store all";
    }

    public void GiveItems()   // comprobar si funciona q a saber 
    {
        int numItems = 0;
        int price = GetCharge();

        if (InventoryController.Instance.GetAmount("Money") >= price)
        {
            foreach(ShopItem s in cart)
            {
                if (s != null && s.item.GetType() == typeof(Item))
                {
                    numItems++;
                }
            }

            if(InventoryController.Instance.itemSpace >= InventoryController.Instance.numItems + numItems)
            {
                InventoryController.Instance.SubstractAmountItem(price, "Money");

                foreach (ShopItem s in cart)
                {
                    if(s!= null)
                    {
                        s.item.amount = s.amountSelected;
                        InventoryController.Instance.AddItem(s.item);
                    }
                    
                }
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

    public void ConfirmSell()
    {
        confirmSell = true;
    }

    public void CartView()
    {
        cartView = !cartView;

        if(cartView)
        {
            for(int i = 0; i < cart.Length; i++)
            {
                if(cart[i]!=null)
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
            for (   ; i < currentShop.stock.Length; i++)
            {
                ShopEntry t = stockUI[i];
                t.gameObject.SetActive(true);
                t.Fill(currentShop.stock[i]);
            }
            for(    ; i < stockUI.Length; i++)
            {
                stockUI[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
