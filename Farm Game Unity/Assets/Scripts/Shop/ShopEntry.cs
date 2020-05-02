using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour
{
    private Image image;
    private Text nameText;
    private Text price;
    private Text stockText;
    private Text amountSelectedText;

    public int position;
    private void Awake()
    {

        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<Text>();
        price = transform.GetChild(2).GetComponent<Text>();
        stockText = transform.GetChild(3).GetComponent<Text>();
        amountSelectedText = transform.GetChild(4).GetComponent<Text>();
    }

    public void Fill(ShopItem s)
    {
        image.sprite = s.item.image;
        nameText.text = s.item.name;
        price.text = s.item.price.ToString();
        stockText.text = s.stock.ToString();
        amountSelectedText.text = s.amountSelected.ToString();

        if (ShopManager.Instance.cartView)
        {
            amountSelectedText.gameObject.SetActive(true);
            stockText.gameObject.SetActive(false);
        }
        else
        {
            stockText.gameObject.SetActive(true);
            amountSelectedText.gameObject.SetActive(false);
        }
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
        else if (Sell.Instance.onShopView )
        {
            Sell.Instance.ReturnItems(position);
        }
    }
}
