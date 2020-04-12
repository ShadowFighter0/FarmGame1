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
    private GameObject select;

    private void Awake()
    { 
        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<Text>();
        price = transform.GetChild(2).GetComponent<Text>();
        stockText= transform.GetChild(3).GetComponent<Text>();
        amountSelectedText = transform.GetChild(4).GetComponent<Text>();
        select = transform.GetChild(5).gameObject;
    }

    public void Fill(ShopItem s)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + s.item.imagePath);
        image.sprite = sprite;
        nameText.text = s.item.name;
        price.text = s.item.price.ToString();
        stockText.text = s.stock.ToString();
        amountSelectedText.text = s.amountSelected.ToString();
    }

    public void Select()
    {
        select.SetActive(!select.activeInHierarchy);
    }

    public void CloseShop()
    {
        select.SetActive(false);
    }

    public void Price(int price)
    {
        nameText.text = price.ToString();
    }
    
}
