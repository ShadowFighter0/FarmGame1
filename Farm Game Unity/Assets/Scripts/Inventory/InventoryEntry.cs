using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour
{
    public GameObject notActive;
    public Image image;
    public Text nameText;
    public Text amount;
    public Text price;

    public int position;

    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<Text>();
        amount = transform.GetChild(2).GetComponent<Text>();
        notActive = transform.GetChild(4).gameObject;
        price = transform.GetChild(5).GetComponent<Text>();
    }

    public void Fill(InventoryItem it)
    {
        Sprite sprite = DataBase.GetItemSprite(it.image);
        image.sprite = sprite;
        nameText.text = it.name;
        amount.text = it.inventoryAmount.ToString();

        if (Sell.Instance.playerNear)
        {
            price.text = DataBase.GetItem(it.name).price.ToString();
            price.gameObject.SetActive(true);
        }
        else
        {
            price.gameObject.SetActive(false);
        }
    }

    public void Button()
    {
        if (Sell.Instance.playerNear)
        {
            Sell.Instance.Button(position);
            AmountPanel.Instance.gameObject.SetActive(true);
            AmountPanel.Instance.On(int.Parse(amount.text));
        }
        else
        {
            InventoryController.Instance.DeleteItem(position);
        }
    }
}
