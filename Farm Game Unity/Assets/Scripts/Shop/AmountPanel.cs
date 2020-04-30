using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountPanel : MonoBehaviour
{
    public static AmountPanel Instance;

    public bool isOn = false;

    private Slider slider;
    private InputField input;

    int amount;

    private void Awake()
    {
        Instance = this;

        slider = transform.GetChild(3).GetComponent<Slider>();
        input = transform.GetChild(4).GetComponent<InputField>();

        gameObject.SetActive(false);

        
    }

    public void On(int maxCant)
    {
        gameObject.SetActive(true);
        slider.maxValue = maxCant;
        slider.value = maxCant;

        SliderValueChange();
        isOn = true;
    }

    public void Off()
    {
        gameObject.SetActive(false);
        isOn = false;
    }

    public void ConfirmAmount()
    {
        amount = (int)slider.value;
        if (ShopManager.Instance.onShop)
        {
            ShopManager.Instance.ConfirmAmount(amount);
        }
        else 
        {
            if (Sell.Instance.playerNear && InventoryController.Instance.currentPage == 0)
                Sell.Instance.AddItem(amount);
            else if(Sell.Instance.onSell)
                Sell.Instance.ConfirmReturnItems(amount);               
        }
        Off();
    }
    public void SliderValueChange()
    {
        input.text = slider.value.ToString();
    }

    public void InputValueChange()
    {
        slider.value = int.Parse(input.text);
    }
}
