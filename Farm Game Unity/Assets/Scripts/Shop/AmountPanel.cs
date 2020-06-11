using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountPanel : MonoBehaviour
{
    public static AmountPanel Instance;

    public bool isOn = false;

    public Slider slider;
    private InputField input;

    int amount;
    bool inventory = false;

    private void Awake()
    {
        Instance = this;

        slider = transform.GetChild(0).GetChild(2).GetComponent<Slider>();
        input = transform.GetChild(0).GetChild(3).GetComponent<InputField>();

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
        slider.value = 0;
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
        else if(Sell.Instance.playerNear && Sell.Instance.onShopView)
        {
            if (inventory)
                Sell.Instance.AddItem(amount);
            else
                Sell.Instance.ConfirmReturnItems(amount);               
        }
        inventory = false;
        Off();
    }

    public void IsOnInventory(bool isOnInventory)
    {
        inventory = isOnInventory;
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
