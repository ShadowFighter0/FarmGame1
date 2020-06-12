using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopEntry : MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI price;
    private TextMeshProUGUI stockText;
    private TextMeshProUGUI amountSelectedText;

    public int position;
    public bool inventory;
    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        price = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        stockText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        amountSelectedText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    public void Fill(ShopItem s)
    {
        stockText.gameObject.SetActive(false);
        image.sprite = s.item.image;
        nameText.text = s.item.name;
        price.text = s.item.price.ToString();

        if (ShopManager.Instance.cartView)
        {
            amountSelectedText.text = s.amountSelected.ToString();
            amountSelectedText.gameObject.SetActive(true);
        }
        else
        {
            amountSelectedText.gameObject.SetActive(false);
        }
    }

    public void Fill(SellItem s)
    {
        image.sprite = s.item.image;
        nameText.text = s.item.name;
        price.text = s.item.price.ToString();
        stockText.gameObject.SetActive(true);
        stockText.text = s.amount.ToString();
        amountSelectedText.gameObject.SetActive(false);
    }

    public void Button()
    {
        if (ShopManager.Instance.onShop)
        {
            ShopManager.Instance.Select(position);
        }
        else if (Sell.Instance.onShopView)
        {
            AmountPanel.Instance.IsOnInventory(inventory);
            if (inventory)
            {
                Sell.Instance.Button(position);
                AmountPanel.Instance.On(int.Parse(stockText.text), "How much amount do you want to sell ?");
            }
            else
            {
                Sell.Instance.ReturnItems(position);
            }
        }
    }
}
