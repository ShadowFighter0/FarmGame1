﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    public string name;
    public Sprite image;

    public bool isActivate = true;

    public int inventoryAmount = 0 ;

    public InventoryItem(string name, Sprite image)
    {
        this.name = name;
        this.image = image;
    }
    public InventoryItem(string name, Sprite image, bool active)
    {
        this.name = name;
        this.image = image;
        this.isActivate = active;
    }

    public int GetInventoryAmount() { return inventoryAmount; }
    public void AddAmount(int cant) { inventoryAmount += cant; }
    public void SubstractAmount(int cant) { inventoryAmount -= cant; }
}

public class InventoryController : MonoBehaviour
{
    #region ExternalScripts
    QuestController missions;
    private FeedController feed;

    public static InventoryController Instance;     //Singleton
    #endregion

    #region numSeeds
    [HideInInspector] public int numItems = 0;  //num of items in the inventory
    [HideInInspector] public int numSeeds = 0;  //num of seeds in the inventory

    public int cantStackMax = 20;

    public int itemSpace = 20;
    public int seedSpace;
    #endregion

    #region Inventory Array
    InventoryItem[] materials;  // Materials inventory
    InventoryItem[] items;      // Items inventory
    InventoryItem[] seeds;      // Seed inventory
    #endregion

    #region Public External Objects
    [Tooltip("Parent of all tools")] public GameObject tools;
    [Tooltip("Parent of the spin (GUI)")] public GameObject spin;
    [Tooltip("Book")] public GameObject book;
    [Tooltip("Delete Panel")] public GameObject deletePanel;

    [Tooltip("Todas las semillas")] public Item[] seedsItems;
    [Tooltip("Todos los materiales")] public Item[] materialsItems;
    #endregion

    
    #region Pages
    private int currentPage = 0;
    private int oldPage = 0;
    public InventoryEntry[] inventoryEntry; // entry of inventory

    public GameObject[] pages;
    #endregion
    
    int itemSelected; //Item selected in the inventory

    private void Awake()
    {
        Instance = this;    //Singleton
        items = new InventoryItem [itemSpace];

        seedSpace = seedsItems.Length;
        seeds = new InventoryItem [seedSpace];
        materials = new InventoryItem [materialsItems.Length];

        //Start seeds and materials
        for(int i = 0; i < seedsItems.Length; i++)
        {
            seeds[i] = new InventoryItem(seedsItems[i].name, seedsItems[i].image, false);
        }
        for (int i = 0; i < materialsItems.Length; i++)
        {
            materials[i] = new InventoryItem(materialsItems[i].name, materialsItems[i].image, false);
        }
    }

    private void Start()
    {
        missions = QuestController.Instance;
        feed = FindObjectOfType<FeedController>();
    }

    private void Update()
    {
        //TODO change panel of menu
        OpenCloseMenu();
    }

    #region Menu && Visual
    private void OpenCloseMenu()
    {
        if (Input.GetKeyDown(InputManager.instance.Inventory))  //open Menu
        {
            book.SetActive(!book.activeSelf);
            if (book.activeSelf)
            {
                ChangeGui();
                Time.timeScale = 0;
                InputManager.instance.ChangeState(InputManager.States.OnUI);
            }
            else
            {
                Time.timeScale = 1;
                InputManager.instance.ChangeState(InputManager.States.Idle);
            }
        }
    }
    void ChangeGui()
    {
        pages[oldPage].SetActive(false);
        pages[currentPage].SetActive(true);

        switch (currentPage)
        {
            case 0:
                for (int i = 0; i < inventoryEntry.Length; i++)
                {
                    InventoryEntry it = inventoryEntry[i];
                    it.notActive.SetActive(false);
                    if (items[i] != null)
                    {
                        
                        it.gameObject.SetActive(true);
                        it.Fill(items[i]);
                    }
                    else
                    {
                        it.gameObject.SetActive(false);
                    }
                }
                break;

            case 1:
                Transform page = pages[currentPage].transform.GetChild(0);
                for (int i = 0; i < page.childCount; i++)
                {
                    InventoryEntry it = page.GetChild(i).GetComponent<InventoryEntry>();
                    if (i< seeds.Length)
                    {
                        it.gameObject.SetActive(true);
                        InventoryItem seed = seeds[i];

                        it.Fill(seed);
                        it.notActive.SetActive(!seed.isActivate);
                    }
                    else
                    {
                        it.gameObject.SetActive(false);
                    }
                    
                }
                //TODO ernesto lo ha cambiado asiq no esta hecho 
                break;


        }
        oldPage = currentPage;
    }
    #endregion

    #region AddItem/Seed/Material
    public void AddItem(Item newItem)
    {
        //Seed
        if(newItem.GetType() == typeof(Seed))
        {
            AddNewSeed(newItem);
        }
        else if(newItem.GetType() == typeof(Material))
        {
            //item = AddMaterial(); 
        }
        else
        {
            AddNewItem(newItem);
        }

        //item
        GameEvents.Instance.ItemCollected(newItem.name, GetAmount(newItem.name));
        feed.Suscribe(newItem.name, newItem.image, newItem.amount);
    }
    private void AddNewItem(Item newItem)
    {
        List<int> positions = SearchItem(newItem.itemName);
        InventoryItem item = null;
        int amount = newItem.amount;
        bool newInventoryItem = true;

        if (positions!=null && positions.Count >= 0)
        {
            for (int i = 0; i < positions.Count && amount > 0; i++)
            {
                item = items[positions[i]];
                if (item.GetInventoryAmount() + amount <= cantStackMax)
                {
                    item.AddAmount(amount);
                    amount = 0 ;
                }
                else
                {
                    int cantAdd = cantStackMax - item.GetInventoryAmount();
                    amount -= cantAdd;
                    item.AddAmount(cantAdd);
                }
            }

            if (amount != 0)
            {
                newInventoryItem = true;
            }
        }
        if ((positions == null || newInventoryItem) && numItems < items.Length)
        {
            item = items[numItems] = new InventoryItem(newItem.itemName, newItem.image);

            if (newInventoryItem)
                item.AddAmount(amount);
            else
                item.AddAmount(newItem.amount);

            numItems++;
        }
    }
    private void AddNewSeed(Item newItem)
    {
        InventoryItem newSeed = seeds[SearchSeed(newItem.itemName)];
        newSeed.AddAmount(newItem.amount);
        if(!newSeed.isActivate)
        {
            newSeed.isActivate = true;
        }
    }

    public void AddMaterial(GameObject obj)
    {
        //TODO
    }
    #endregion

    #region Delete item
    public void ConfirmDeleteItem()
    {
        if (currentPage == 0)
        {
            items[itemSelected] = null;
            ReOrderItem();
        }
        else if (currentPage == 1)
        {
            seeds[itemSelected].SubstractAmount(seeds[itemSelected].GetInventoryAmount());
        }
            
        deletePanel.SetActive(false);
        
        ChangeGui();
    }

    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }

    private void ReOrderItem()
    {
        //Ernesto lo ha cambiado asiq no esta hecho 
    }
    #endregion

    #region Public Functions && General Functions

    /// <summary>
    /// Please check first if there's enoguht amount of the item witch GetAmount() 
    /// </summary>
    /// <param name="cant">Amount to substract.</param>
    /// <param name="id">Name of item</param>
    public void SubstractAmountItem(int cant, string id)
    {
        List<int> positions = SearchItem(id);
        int cantidadPorRestar = cant;
        if(positions!=null)
        {
            for (int i = 0; i < positions.Count && cantidadPorRestar > 0; i++)
            {
                InventoryItem item = items[positions[i]];

                if (item.GetInventoryAmount() >= cantidadPorRestar)
                {
                    item.SubstractAmount(cantidadPorRestar);
                    cantidadPorRestar -= item.GetInventoryAmount();

                    if (item.GetInventoryAmount() == 0)
                    {
                        items[positions[i]] = null;
                    }
                }
                else
                {
                    item.SubstractAmount(item.GetInventoryAmount());
                    cantidadPorRestar -= item.GetInventoryAmount();
                    items[positions[i]] = null;
                }
            }

            ReOrderItem();
        }
    }

    /// <summary>
    /// Please Check first the amount
    /// </summary>
    /// <param name="cant"></param>
    /// <param name="id"></param>
    public void SubstractAmountSeed(int cant, string id)
    {
        InventoryItem item = seeds[SearchSeed(id)];
        item.SubstractAmount(cant);
        GameEvents.Instance.ItemCollected(item.name, item.GetInventoryAmount());
    }

    public void ChangePage(int page)
    {
        currentPage = page;
        ChangeGui();
    }
    public void DeleteItem(int pos)
    {
        itemSelected = pos;
        deletePanel.SetActive(true);
    }

    // Return the current amount in inventory of an item
    public int GetAmount(string name)
    {
        List<int> positions = SearchItem(name);
        if (positions!= null && positions.Count > 0)
        {
            int cant = 0;
            for (int i = 0; i < positions.Count; i++)
            {
                InventoryItem item = items[positions[i]];
                cant += item.GetInventoryAmount();
            }
            return cant;
        }
        else 
        {
            int pos = SearchSeed(name);
            if (pos >= 0)
            {
                return seeds[pos].GetInventoryAmount();
            }
            else
            {
                pos = SearchMaterial(name);
                if (pos >= 0)
                {
                    return materials[pos].GetInventoryAmount();
                }
                else
                {
                    return 0;
                }
            }
        }
    }
    #endregion

    #region Search Functions

    /// <summary>
    /// Search for an Item (return pos in array or -1 if there's no in inventory)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private List<int> SearchItem(string name)  //Todo return an array of ints with all positions
    {
        List<int> positions = new List<int>();
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            {
                if (name.Equals(items[i].name))
                    positions.Add(i);
            }
        }
        if (positions.Count > 0)
            return positions;
        else
            return null;
    }
    private int SearchSeed(string name)
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if (name.Equals(seeds[i].name))
            {
                return i;
            }
        }
        return -1;
    }
    private int SearchMaterial(string name)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (name.Equals(materials[i].name))
                return i;
        }
        return -1;
    }
    #endregion

    #region TODO
    //TODO
    //TODO comprar items
    //TODO function that sell items (give item + delete)
    //TODO function that manage money
    //TODO notes in gameplay for missions
    //TODO spin / quick acces
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        ///Player tiene un collider le pasa el script ITEM cuando pase por encima y despues lo destruye
        AddMaterial(other.gameObject);
        Destroy(other.gameObject);
    }
}
