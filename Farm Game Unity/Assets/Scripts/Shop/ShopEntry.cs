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

    public void Price(int price)
    {
        nameText.text = price.ToString();
    }

    public void Button()
    {
        if (ShopManager.Instance.onShop)
        {
            ShopManager.Instance.Select(position);
        }
        else if (Sell.Instance.onShopView)
        {
            Sell.Instance.ReturnItems(position);
        }
    }
}
