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
        currentShop.stock[pos].Select();
        stockUI[pos].Select();
        slider.maxValue = currentShop.stock[pos].stock;
        amountPanel.SetActive(true);
    }

    public void ConfirmAmount ()
    {
        currentShop.stock[pos].amountSelected = (int)slider.value;
        amountPanel.SetActive(false);
        slider.value = 0;
        cart[numCart] = currentShop.stock[pos];
        numCart++;
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
            if(i < currentShop.stock.Length-1)
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
    private void GetCharge() // Comprobar si funciona q ha saber
    {
        int price = 0;
        for(int i = 0; i < cart.Length && cart[i]!=null; i++)
        {
            price += cart[i].item.price;
        }
        totalPrice.text = price.ToString();

        if(confirmSell)
        {
            GiveItems(price);
        }
    }

    private void NotEnoughtMoney()
    {
        //popUp no hay pasta reutilizar panel si se puede 
    }
    private void NotEnoughtSpace()
    { 
        //PopUp no hay espacio reutilizar amount panel si se puede
    }

    private void GiveItems(int price)   // comprobar si funciona q a saber 
    {
        int numSeeds = 0;
        int numItems = 0;
        if (InventoryController.Instance.GetAmount("Money") > price)
        {
            foreach(ShopItem s in cart)
            {
                if (s.item.GetType() == typeof(Seed))
                {
                    numSeeds++;
                }
                else if (s.item.GetType() == typeof(Material))
                {
                    //Ernesto io t quero  
                }
                else
                {
                    numItems++;
                }
            }

            if(InventoryController.Instance.seedSpace > InventoryController.Instance.numSeeds + numSeeds &&
               InventoryController.Instance.itemSpace > InventoryController.Instance.numItems + numItems)
            {
                InventoryController.Instance.SubstractAmountItem(price, "Money");

                foreach (ShopItem s in cart)
                {
                    s.item.amount = s.amountSelected;
                    InventoryController.Instance.AddItem(s.item);
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
        Debug.Log(currentShop);
        cartView = !cartView;

        if(cartView)
        {
            for(int i = 0; i < cart.Length; i++)
            {
                if(cart[i]!=null)
                {
                    stockUI[i].Fill(cart[i]);
                    stockUI[i].gameObject.SetActive(true);
                }
                else
                {
                    stockUI[i].gameObject.SetActive(false);
                }
            }
            GetCharge();
        }
        else
        {
            int i = 0;
            for (   ; i < currentShop.stock.Length; i++)
            {
                stockUI[i].gameObject.SetActive(true);
                stockUI[i].Fill(currentShop.stock[i]);
            }
            for(    ; i < stockUI.Length; i++)
            {
                stockUI[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
