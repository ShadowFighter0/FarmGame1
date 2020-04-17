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
    public void CloseShop()
    {
        isSelected = true;
    }
    public void Fill(int amount)
    {
        stock = amount;
    }
    public void ToCart (int i)
    {
        amountSelected = i;
    }
}


public class Shop : MonoBehaviour
{
    public Item[] stockItems;

    public ShopItem[] stock;

    private void Start()
    {
        stock = new ShopItem[stockItems.Length];
        
        for(int i = 0; i < stockItems.Length; i++)
        {
            stock[i].item = stockItems[i];
            stock[i].Fill(GenerateAmount());
        }
    }

    private int GenerateAmount()
    {
        // q este mejor si eso 
        return Random.Range(3, 10);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            ShopManager.Instance.SetCurrentShop(this);
        }
    }

    public void NewDay()
    {
        foreach(ShopItem s in stock)
        {
            s.Fill(GenerateAmount());
        }
    }


}
