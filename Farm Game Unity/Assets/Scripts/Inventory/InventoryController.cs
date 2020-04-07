using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    public string name;
    public string description;
    public Sprite image;

    public int inventoryAmount;

    public InventoryItem(string name, Sprite image)
    {
        this.name = name;
        this.image = image;
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
    public int numItems = 0;  //num of items in the inventory
    public int numSeeds = 0;  //num of seeds in the inventory

    public int cantStackMax = 20;

    public int itemSpace = 20;
    public int seedSpace = 10;
    public int materialsSpace = 3;
    #endregion

    #region Inventory Array
    InventoryItem[] materials;// Materials inventory
    InventoryItem[] items; // Items inventory
    InventoryItem[] seeds; // Seed inventory
    #endregion

    #region Public External Objects
    [Tooltip("Parent of all tools")] public GameObject tools;
    [Tooltip("Parent of the spin (GUI)")] public GameObject spin;
    [Tooltip("Parent of all materials (GUI)")] public GameObject materialsGUI;
    [Tooltip("Book")] public GameObject book;
    [Tooltip("Delete Panel")] public GameObject deletePanel;
    public Image cursor;
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
        seeds = new InventoryItem [seedSpace];
        materials = new InventoryItem [materialsSpace];
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
        if (Input.GetKeyDown(KeyCode.Tab))  //open Menu
        {
            book.SetActive(!book.activeSelf);
            if (book.activeSelf)
            {
                ChangeGui();
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
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
                    if (items[i] != null)
                    {
                        inventoryEntry[i].gameObject.SetActive(true);
                        inventoryEntry[i].Fill(items[i]);
                    }
                    else
                    {
                        inventoryEntry[i].gameObject.SetActive(false);
                    }
                }
                break;

            case 1:
                for(int i = 0; i < pages[1].transform.GetChild(0).childCount;i++ )
                {
                    if (seeds[i] != null)
                    {
                        pages[1].transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                        pages[1].transform.GetChild(0).GetChild(i).GetComponent<InventoryEntry>().Fill(seeds[i]);
                    }
                    else
                    {
                        pages[1].transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;


        }
        oldPage = currentPage;
    }
    #endregion

    #region AddItem/Seed/Material
    public void AddItem(Item newItem)
    {
        //Todo not Stack till Mathf.infinite
        InventoryItem item = null;
        //Seed
        if(newItem.GetType() == typeof(Seed))
        {
            item = AddNewSeed(newItem);
        }
        else if(newItem.GetType() == typeof(Material))
        {
            //Ernesto tonto tonto puto tonto 
        }
        else
        {
            item = AddNewItem(newItem);
        }

        //item
        GameEvents.Instance.ItemCollected(item.name, item.GetInventoryAmount());
        feed.Suscribe(item.name, item.image, newItem.amount);
    }
    private InventoryItem AddNewItem(Item newItem)
    {
        int pos = SearchItem(newItem.itemName);
        InventoryItem item = null;

        if (pos >= 0)
            item = items[pos];

        if (item != null && (item.inventoryAmount + newItem.amount < cantStackMax))
        {
                item.AddAmount(newItem.amount);
        }
        else if (numItems < items.Length)
        {
            items[numItems] = new InventoryItem(newItem.itemName, newItem.image);
            item = items[numItems];
            item.AddAmount(newItem.amount);
            numItems++;
        }
        return item;
    }
    private InventoryItem AddNewSeed(Item newItem)
    {
        int pos = SearchSeed(newItem.itemName);
        InventoryItem seed = null;

        if (pos >= 0)
            seed = seeds[pos];

        if (seed != null)
        {
            seed.AddAmount(newItem.amount);
        }
        else if (numSeeds < seeds.Length)
        {
            seeds[numSeeds] = new InventoryItem(newItem.itemName, newItem.image);
            seed = seeds[numSeeds];
            seed.AddAmount(newItem.amount);
            numSeeds++;
        }
        return seed;
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
            seeds[itemSelected] = null;
            ReOrderSeed();
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
        for(int i = 0; i < items.Length; i++)
        {
            InventoryItem item = items[i];
            for(int j = i + 1; i < items.Length; i++)
            {
                
            }
        }
//Agrupar cosas
        
        for (int i = itemSelected; i < items.Length; i++)
        {
            if(i!=items.Length-1)
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

    private void ReOrderSeed()
    {
        for (int i = itemSelected; i < seeds.Length; i++)
        {
            if (i != seeds.Length - 1)
            {
                seeds[i] = seeds[i + 1];
            }
            else
            {
                seeds[i] = null;
            }
        }
        numSeeds--;
    }
    #endregion

    #region Public Functions && General Functions
    public void AddMaterial(GameObject obj)
    {
        feed.Suscribe(obj.tag, obj.GetComponent<Image>().sprite, obj.GetComponent<Item>().amount); //pop that show player what you have obtain

        Item it = obj.GetComponent<Item>();
        switch (it.itemName) //TODO change this 
        {
            case "Wood":
                // pos = 0;
                break;
            case "Stone":
                // pos = 1;
                break;
            default:
                // pos = 2;
                break;
        }

        //materiales[pos] += it.amount;
    }

    public void SubstractAmountItem(int cant, string id)
    {
        int pos = SearchItem(id);
        if(pos >= 0)
        {
            InventoryItem item = items[pos];
            item.SubstractAmount(cant);
            GameEvents.Instance.ItemCollected(item.name, item.GetInventoryAmount());

            if (item.inventoryAmount <= 0)
            {
                items[pos] = null;
                ReOrderItem();
            }
        }
        
    }
    public void SubstractAmountSeed(int cant, string id) //De cual, hay muchos bro? 
    {
        int pos = SearchSeed(id);
        if (pos >= 0)
        {
            InventoryItem item = seeds[pos];
            item.SubstractAmount(cant);
            GameEvents.Instance.ItemCollected(item.name, item.GetInventoryAmount());

            if (item.inventoryAmount <= 0)
            {
                seeds[pos] = null;
                ReOrderSeed();
            }
        }
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
    public int GetAmount(string name, string type)
    {
        if(type.Equals("Item")) // total cant
        {
            int pos = SearchItem(name);
            if (pos >= 0)
            {
                InventoryItem item = items[pos];
                return item.GetInventoryAmount();
            }
            else
            {
                Debug.Log("no esta en el inventario compa");
                return 0;
            }      
        }
        else if(type.Equals("Seed"))
        {
            int pos = SearchSeed(name);
            if (pos >= 0)
            {
                InventoryItem item = seeds[pos];
                return item.GetInventoryAmount();
            }
            else
            {
                Debug.Log("no esta en el inventario compa");
                return 0;
            }
        }
        return 0;
    }
    #endregion

    #region Search Functions

    //Search for an Item (return pos in array or -1 if there's no in inventory)
    public int SearchItem(string name)  //Todo return an array of ints with all positions
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            {
                if (name.Equals(items[i].name))
                    return i;
            }
        }
        return -1;
    }
    public int SearchSeed(string name)
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i] != null)
            {
                if (name.Equals(seeds[i].name))
                    return i;
            }
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
