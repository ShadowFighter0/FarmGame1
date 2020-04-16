using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public Item item;

    public int stock;
    public int amountSelected;
    bool isSelected = false;

    public void Select()
    {
        isSelected = !isSelected;
    }

    /// <summary>
    /// Restart the isSelected variable
    /// </summary>
    public void CloseShop()
    {
        isSelected = true;
    }
}

public class ShopManager : MonoBehaviour
{
    #region Variables
    public GameObject shopPanel;
    public GameObject amountPanel;
    public ShopEntry totalPrice;

    private ShopItem[] stockItems; //Items that can be bought


    private ShopEntry[] stockUI; // VisualEntrys
    
    private ShopItem[] cart; //Items that will be added to your inventory

    private bool confirmSell = false; //Button that confirm the sell or buy 
    private bool cartView;
    #endregion


    #region Functions
    private void Start()
    {
        cart = new ShopItem[stockItems.Length];
    }
    /// <summary>
    /// Button click select an item
    /// </summary>
    /// <param name="pos"></param>
    public void Select(int pos)
    {
        stockItems[pos].Select();
        stockUI[pos].Select();
    }

    /// <summary>
    /// Open the shop's panel and Time.scaleTime = 0;
    /// </summary>
    public void OpenShop()
    {
        cartView = true;
        CartView();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        shopPanel.SetActive(true);
    }

    /// <summary>
    /// Close the shop's panel and Time.scaleTime = 1
    /// </summary>
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        for(int i  = 0; i < stockUI.Length; i++)
        {
            stockUI[i].CloseShop();
            if(i < stockItems.Length-1)
            {
                stockItems[i].CloseShop();
            }
        }
        confirmSell = false;
    }

    /// <summary>
    /// Get Items from Assets 
    /// </summary>
    /// 
    public void ReFillShop()//newDay
    {
        //pillar items de Assets
        foreach(ShopItem s in stockItems)
        {
            //s.item =
            //s.amountPanel = 
        }
    }

    /// <summary>
    ///Once you have selected all the items you wanna buy get the total price and give the items 
    /// </summary>
    private void GetCharge() // 
    {
        int price = 0;
        for(int i = 0; i< cart.Length; i++)
        {
            price += cart[i].item.price;
        }
        totalPrice.Price(price);

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

    private void GiveItems(int price)
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
            for (int i = 0; i < stockItems.Length; i++)
            {
                stockUI[i].Fill(stockItems[i]);
            }
        }
    }
    #endregion
}
