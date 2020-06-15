﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
   public void Close()
   {
        if (ShopManager.Instance.onShop)
        {
            ShopManager.Instance.CloseShop();
            InteractButton.Instance.pauseButton.SetActive(true);
            InteractButton.Instance.waterButton.SetActive(true);
            InteractButton.Instance.digButton.SetActive(true);
        }
        else if (Sell.Instance.playerNear && Sell.Instance.onShopView)
        {
            Sell.Instance.CloseSell();
        }
        else if (InteractButton.Instance.workShopController != null && !InteractButton.Instance.workShopController.finished)
        {
            InteractButton.Instance.workShopController.DisableCamera();
            InteractButton.Instance.workShopController.ChangeItemsState(false);
        }
        InteractButton.Instance.gameObject.SetActive(true);
    }
}