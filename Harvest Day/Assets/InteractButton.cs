using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : MonoBehaviour
{
    public bool shop;
    Shop currentShop;

    public bool sell;

    public bool dialogue;
    NpcController currentNpc;

    public bool questController;

    public bool workshop;
    public WorkShopController workShopController;

    public bool mail;

    public static InteractButton Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Click()
    {
        if(shop)
        {
            ShopManager.Instance.OpenShop(currentShop);
            
        }
        else if (sell)
        {
            Sell.Instance.OpenInicialMenu();
        }
        else if (dialogue)
        {
            currentNpc.StartDialogue();
        }
        else if (questController)
        {
            QuestController.Instance.UpdatePanels();
            InventoryController.Instance.OpenMenu();
            InventoryController.Instance.ChangePage(2);
        }
        else if (workshop)
        {
            workShopController.finished = false;
            workShopController.UIMenu.SetActive(true);
            InputManager.instance.ChangeState(InputManager.States.OnUI);
        }
        else if (mail)
        {
            MailBoxController.instance.mailsPanel.SetActive(true);
            InputManager.instance.ChangeState(InputManager.States.OnUI);
            MailBoxController.instance.done = false;
        }

        gameObject.SetActive(false);
    }

    public void Shop(Shop s)
    {
        currentShop = s;
        if (shop)
        {
            shop = false;
        }
        else
        {
            shop = true;
        }
    }

    public void Npc (NpcController npc)
    {
        currentNpc = npc;
        if (dialogue)
        {
            dialogue = false;
        }
        else
        {
            dialogue = true;
        }
    }

    public void Workshop(WorkShopController work)
    {
        workShopController = work;
        if (workshop)
        {
            workshop = false;
        }
        else
        {
            workshop = true;
        }
    }
}
