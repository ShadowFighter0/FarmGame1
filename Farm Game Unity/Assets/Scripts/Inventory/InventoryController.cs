using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public string image;

    public int price;

    public bool isActivate = true;

    public int inventoryAmount = 0;

    public InventoryItem(string name, string image)
    {
        this.name = name;
        this.image = image;
    }
    public InventoryItem(string name, string image, bool active)
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

    [HideInInspector] public int numItems = 0;  //num of items in the inventory
    public int cantStackMax = 20;

    public int itemSpace = 20;
    public int seedSpace = 20;

    #region Inventory Array
    InventoryItem[] items;      // Items inventory
    InventoryItem[] seeds;      // Seed inventory
    #endregion

    #region Public External Objects
    [Tooltip("Book")] public GameObject book;
    [Tooltip("Delete Panel")] public GameObject deletePanel;

    private Item[] seedsItems;
    [HideInInspector] public bool inventoryOpen = false;

    InputManager.States currentState;
    #endregion

    #region Pages
    public int currentPage = 0;
    public InventoryEntry[] inventoryEntry; //Entry of inventory
    public GameObject questFolder;
    #endregion

    int itemSelected; //Item selected in the inventory

    private void Awake()
    {
        seedsItems = Resources.LoadAll<Seed>("Data/Items");
        Instance = this;    //Singleton
        items = new InventoryItem[itemSpace];

        seedSpace = seedsItems.Length;
        seeds = new InventoryItem[seedSpace];

        //Start seeds 
        for (int i = 0; i < seedsItems.Length; i++)
        {
            seeds[i] = new InventoryItem(seedsItems[i].name, seedsItems[i].image.name, false);
        }
    }

    private void Start()
    {
        book.SetActive(false);
        missions = QuestController.Instance;
        feed = FindObjectOfType<FeedController>();
        GameEvents.OnSaveInitiated += Save;
        if (SaveLoad.SaveExists("InventorySeeds"))
        {
            InventoryItem[] savedItems = SaveLoad.Load<InventoryItem[]>("InventorySeeds");
            for (int i = 0; i < savedItems.Length; i++)
            {
                seeds[i] = savedItems[i];
            }
        }
        if (SaveLoad.SaveExists("InventoryItems"))
        {
            InventoryItem[] savedItems = SaveLoad.Load<InventoryItem[]>("InventoryItems");
            for (int i = 0; i < savedItems.Length; i++)
            {
                items[i] = savedItems[i];
            }
        }
    }

    private void Save()
    {
        SaveLoad.Save(items, "InventoryItems");
        SaveLoad.Save(seeds, "InventorySeeds");
    }

    #region Menu && Visual
    public void OpenMenu()
    {
        inventoryOpen = true;
        book.SetActive(true);
        ChangeGui();
        currentState = InputManager.state;
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }
    public void CloseMenu()
    {
        inventoryOpen = false;
        book.SetActive(false);
        InputManager.instance.ChangeState(currentState);
    }

    void ChangeGui()
    {
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
                if(questFolder.activeSelf)
                {
                    questFolder.SetActive(false);
                }
                break;

            case 1:
                for (int i = 0; i < inventoryEntry.Length; i++)
                {
                    InventoryEntry it = inventoryEntry[i];
                    if (i < seeds.Length)
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
                if (questFolder.activeSelf)
                {
                    questFolder.SetActive(false);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryEntry.Length; i++)
                {
                    inventoryEntry[i].gameObject.SetActive(false);
                }

                if (!questFolder.activeSelf)
                {
                    questFolder.SetActive(true);
                }
                break;
        }
    }
    #endregion

    public string GetID(int pos)
    {
        return items[pos].name;
    }

    #region AddItem/Seed
    public void AddItem(Item newItem)
    {
        if (newItem.GetType() == typeof(Seed))
        {
            AddNewSeed(newItem);
            SeedPlanter.instance.UpdateCurrentSeeds();
        }
        else
        {
            if (newItem.name == "Money")
            {
                List<int> positions = SearchItem("Money");

                if (positions != null)
                {
                    items[positions[0]].AddAmount(newItem.amount);
                }
                else if (numItems < items.Length)
                {
                    items[numItems] = new InventoryItem(newItem.itemName, newItem.image.name);
                    items[numItems].AddAmount(newItem.amount);
                    numItems++;
                }
            }
            else
            {
                AddNewItem(newItem);
            }
        }

        //item
        GameEvents.ItemCollected(newItem.name, GetAmount(newItem.name));
        QuestController.Instance.UpdatePanels();
        feed.Suscribe(newItem.name, newItem.image, newItem.amount);
    }
    private void AddNewItem(Item newItem)
    {
        List<int> positions = SearchItem(newItem.itemName);
        InventoryItem item = null;
        int amount = newItem.amount;

        if (positions != null && positions.Count > 0)
        {
            Debug.Log("Sumar cant Item");
            for (int i = 0; i < positions.Count && amount > 0; i++)
            {
                item = items[positions[i]];
                if (item.GetInventoryAmount() + amount <= cantStackMax)
                {
                    item.AddAmount(amount);
                    amount = 0;
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
                item = items[numItems] = new InventoryItem(newItem.itemName, newItem.image.name);
                item.AddAmount(amount);
            }
        }
        if (positions == null && numItems < items.Length)
        {
            Debug.Log("Nuevo Item");
            int numEntrys = amount / cantStackMax;
            Debug.Log(numEntrys);
            int off = amount % cantStackMax;
            Debug.Log(off);

            for (int i = 0; i < numEntrys; i++)
            {
                item = items[numItems] = new InventoryItem(newItem.itemName, newItem.image.name);
                item.AddAmount(20);
                numItems++;
            }
            if (off > 0)
            {
                item = items[numItems] = new InventoryItem(newItem.itemName, newItem.image.name);
                item.AddAmount(off);
                numItems++;
            }
        }
    }
    private void AddNewSeed(Item newItem)
    {
        InventoryItem newSeed = seeds[SearchSeed(newItem.itemName)];
        newSeed.AddAmount(newItem.amount);

        if (!newSeed.isActivate)
        {
            newSeed.isActivate = true;
        }
    }
    #endregion

    #region Delete item
    public void ConfirmDeleteItem()
    {
        if (currentPage == 0)
        {
            items[itemSelected] = null;
            ReOrderSimple(itemSelected);
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
        List<InventoryItem> length = NumberOfItems();

        for (int i = 0; i < length.Count; i++)
        {
            if (length[i] != null)
                ManageReorder(length[i]);
        }
    }

    /// <summary>
    /// todos los tipos de items q hay en el inventory
    /// </summary>
    /// <returns></returns>
    private List<InventoryItem> NumberOfItems()
    {
        List<InventoryItem> it = new List<InventoryItem>();
        for (int i = 0; i < items.Length; i++)
        {
            bool stop = false;
            if (i == 0)
            {
                it.Add(items[0]);
            }
            for (int j = 0; j < it.Count && !stop; j++)
            {
                if (items[i] != it[j])
                {
                    it.Add(items[i]);
                    stop = true;
                }
            }
        }
        return it;
    }

    private void ManageReorder(InventoryItem item)
    {
        List<int> positions = SearchItem(item.name);
        if (positions != null)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                InventoryItem aux = items[positions[i]];
                int offset = cantStackMax - aux.GetInventoryAmount();

                if (offset != 0)
                {
                    for (int j = i + 1; j < positions.Count && offset > 0; j++)
                    {
                        int cant = items[positions[j]].GetInventoryAmount();

                        if (cant > offset)
                        {
                            aux.AddAmount(offset);
                            items[positions[j]].SubstractAmount(offset);
                            offset = 0;
                        }

                        else if (cant == offset)
                        {
                            aux.AddAmount(offset);
                            items[positions[j]] = null;
                            ReOrderSimple(positions[j]);
                            positions = SearchItem(item.name);
                        }

                        else if (cant < offset)
                        {
                            offset -= cant;
                            items[positions[j]] = null;
                            ReOrderSimple(positions[j]);
                            positions = SearchItem(item.name);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// items[i] = items[i+1] y numItems--
    /// </summary>
    /// <param name="pos"></param>
    private void ReOrderSimple(int pos)
    {
        for (int i = pos; i < items.Length; i++)
        {
            if (i != items.Length - 1)
            {
                items[i] = items[i + 1];
            }
            else
            {
                items[i] = null;
            }
        }
        numItems--;
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
        if (positions != null)
        {
            for (int i = 0; i < positions.Count && cantidadPorRestar > 0; i++)
            {
                InventoryItem item = items[positions[i]];

                if (item.GetInventoryAmount() >= cantidadPorRestar)
                {
                    item.SubstractAmount(cantidadPorRestar);
                    cantidadPorRestar = 0;

                    if (item.GetInventoryAmount() == 0)
                    {
                        items[positions[i]] = null;
                        ReOrderSimple(positions[i]);
                        for (int j = i; j < positions.Count; j++)
                            positions[j]--;
                    }
                }
                else
                {
                    item.SubstractAmount(item.GetInventoryAmount());
                    cantidadPorRestar -= item.GetInventoryAmount();
                    items[positions[i]] = null;
                    for (int j = i; j < positions.Count; j++)
                        positions[j]--;
                }
            }
        }
        ReOrderItem();
        ChangeGui();

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
        GameEvents.ItemCollected(item.name, item.GetInventoryAmount());
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
        if (positions != null && positions.Count > 0)
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
        }
        return 0;
    }
    #endregion

    #region Search Functions

    /// <summary>
    /// Search for an Item (return positions in a list or null if there's no in inventory)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private List<int> SearchItem(string name)  //Todo return an array of ints with all positions
    {
        List<int> positions = new List<int>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && name.Equals(items[i].name))
            {
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
    #endregion

}
