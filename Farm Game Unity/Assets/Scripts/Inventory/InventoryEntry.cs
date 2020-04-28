using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour
{
    [HideInInspector] public GameObject notActive;
    private Image image;
    private Text nameText;
    private Text amount;
    private Text price;

    public int position;

    // Start is called before the first frame update
    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<Text>();
        amount = transform.GetChild(3).GetComponent<Text>();
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
            AmountPanel.Instance.gameObject.SetActive(true);
            AmountPanel.Instance.On(int.Parse(amount.text));
        }
        else
        {
            InventoryController.Instance.DeleteItem(position);
        }
    }
}
