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

    public int position;

    // Start is called before the first frame update
    private void Awake()
    {
        notActive = transform.GetChild(4).gameObject;
        image = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<Text>();
        amount = transform.GetChild(2).GetComponent<Text>();
    }
    public void Fill(InventoryItem it)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/" + it.image);
        image.sprite = sprite;
        nameText.text = it.name;
        amount.text = it.inventoryAmount.ToString();
    }

    public void Button()
    {
        if(Sell.Instance.playerNear)
        {
            Sell.Instance.AddItem(position);
        }
        else
        {
            InventoryController.Instance.DeleteItem(position);
        }
    }
}
