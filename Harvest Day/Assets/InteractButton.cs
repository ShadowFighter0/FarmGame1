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

    public GameObject pauseButton;
    public GameObject waterButton;
    public GameObject digButton;

    public bool hoe;
    Hoe hoeController;
    public bool water;
    WateringCan waterController;

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
            pauseButton.SetActive(false);
            waterButton.SetActive(false);
            digButton.SetActive(false);

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
        else if (hoe)
        {
            hoeController.Function();
        }
        else if (water)
        {
            InputManager.instance.playerAnim.SetTrigger("MultiWatering");
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

    public void Hoe (Hoe h)
    {
        hoeController = h;
        if (hoe)
        {
            hoe = false;
        }
        else
        {
            hoe = true;
        }
    }

    public void Water(WateringCan w)
    {
        waterController = w;
        if (water)
        {
            water = false;
        }
        else
        {
            water = true;
        }
    }
}
