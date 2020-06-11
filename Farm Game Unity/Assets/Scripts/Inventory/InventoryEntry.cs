using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour
{
    public GameObject notActive;
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI price;

    public int position;

    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        amount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        notActive = transform.GetChild(4).gameObject;
        price = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    public void Fill(InventoryItem it, int mode)
    {
        image.enabled = true;
        Sprite sprite = DataBase.GetItemSprite(it.image);
        image.sprite = sprite;
        if(mode == 0)
        {
            nameText.text = it.name;
        }
        else
        {
            Seed s = SeedPlanter.instance.GetSeed(it.name);
            nameText.text = s.food.name + " seed";
        }
        amount.text = it.inventoryAmount.ToString();
    }

    
    public void Fill()
    {
        image.enabled = false;
        nameText.text = "";
        amount.text = "";       
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
