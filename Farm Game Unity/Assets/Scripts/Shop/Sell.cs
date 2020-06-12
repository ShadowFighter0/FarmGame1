using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem
{
    public Item item;

    public int amountSelected = 0;
    public int amount = 0;

    public SellItem(Item i, int stock)
    {
        item = i;
        amount = stock;
    }
}

public class Sell : MonoBehaviour
{
    SellItem[] stock;
    ShopEntry[] stockUI;
    ShopEntry[] inventoryUI = new ShopEntry[21];

    int numStock = 0;
    int position = 0;
    int moneyAmount;
    public Item moneyClass;
    public GameObject UiIcon;

    public bool playerNear;
    public bool onShopView;
    public bool onInitialMenu;

    public GameObject shopPanel;
    public GameObject inventoryPanel;

    public GameObject inicialMenu;

    public static Sell Instance;
    private OutlineController outline;

    private void Start()
    {
        Instance = this;
        stockUI = ShopManager.Instance.stockUI;
        Transform aux = inventoryPanel.transform.GetChild(1).GetChild(0);
        for (int i = 0; i < aux.childCount; i++)
        {
            inventoryUI[i] = aux.GetChild(i).GetComponent<ShopEntry>();
        }
        stock = new SellItem[stockUI.Length];
        outline = GetComponent<OutlineController>();
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(InputManager.instance.Interact) && !onShopView && !onInitialMenu)
        {
            OpenInicialMenu();
        }
        if ((onInitialMenu || onShopView) && Input.GetKeyDown(InputManager.instance.Escape))
        {
            if (onShopView)
            {
                CloseSell();
            }
            else if (onInitialMenu)
            {
                CloseInitialMenu();
            }
        }

    }

    public void OpenInicialMenu()
    {
        inicialMenu.SetActive(true);
        onInitialMenu = true;
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }

    public void CloseInitialMenu()
    {
        inicialMenu.SetActive(false);
        onInitialMenu = false;
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    /// <summary> 
    /// Abre o cierra la tienda (UI) si usas la E 
    /// </summary> 
    public void OpenSell()
    {
        onInitialMenu = false;
        onShopView = true;

        InputManager.instance.ChangeState(InputManager.States.OnUI);

        //oculta cosas de la store 
        shopPanel.transform.GetChild(0).gameObject.SetActive(false);
        shopPanel.transform.GetChild(4).gameObject.SetActive(false);
        shopPanel.transform.GetChild(5).gameObject.SetActive(false);
        shopPanel.transform.GetChild(6).gameObject.SetActive(false);


        shopPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        ShowStock();
    }

    /// <summary> 
    /// Si abres el inventario cuando estas en el Sell se apaga  
    /// </summary> 
    public void CloseSell()
    {
        onShopView = false;
        InputManager.instance.ChangeState(InputManager.States.Idle);
        shopPanel.SetActive(false);
        inventoryPanel.SetActive(false);

        //desoculta cosas de la store 
        shopPanel.transform.GetChild(0).gameObject.SetActive(true);
        shopPanel.transform.GetChild(4).gameObject.SetActive(true);
        shopPanel.transform.GetChild(5).gameObject.SetActive(true);
        shopPanel.transform.GetChild(6).gameObject.SetActive(true);
    }



    /// <summary> 
    /// Añadir dinero a la "caja" 
    /// </summary> 
    /// <param name="cant"></param> 
    public void AddMoney(int cant)
    {
        moneyAmount += cant;
        //TODO q salga bonico esto 

        UiIcon.SetActive(true);
        StartCoroutine(UiIconOff());

    }

    /// <summary> 
    /// Boton de collect Money  
    /// </summary> 
    public void CollectMoney()
    {
        Item aux = moneyClass;
        aux.amount = moneyAmount;
        InventoryController.Instance.AddItem(aux);
        moneyAmount = 0;
    }

    /// <summary> 
    /// Presionas un boton de la tienda para devolverlo al inventario (luego entra a ConfirmReturnItems) 
    /// </summary> 
    /// <param name="pos"></param> 
    public void ReturnItems(int pos)
    {
        position = pos;
        AmountPanel.Instance.On(stock[position].amount, "How much amount do you want to recover?");
    }

    /// <summary> 
    /// desues de usar "Button" usas esta funcion para añadir cuanta cantidad quieres recuperar del item  
    /// </summary> 
    /// <param name="cant"></param> 
    public void ConfirmReturnItems(int cant)
    {
        Item i = stock[position].item;
        i.amount = cant;
        InventoryController.Instance.AddItem(i);

        stock[position].amount -= cant;
        if (stock[position].amount == 0)
        {
            UpdateBoxDisable(i.itemName);
            ReOrder();
            numStock--;
        }
        ShowStock();
    }

    /// <summary> 
    /// si eliminas un item de una posicion x todos los objetos por debajo haran x-1 
    /// </summary> 
    private void ReOrder()
    {
        for (int i = position; i < stock.Length && stock[i] != null; i++)
        {
            if (i != stock.Length - 1)
            {
                stock[i] = stock[i + 1];
            }
            else
            {
                stock[i] = null;
            }
        }
    }

    /// <summary> 
    /// actualiza el panel de la tienda para mostrar los items que estan disponibles para vender 
    /// </summary> 
    private void ShowStock()
    {
        InventoryItem[] aux = InventoryController.Instance.AllItems();

        for (int i = 0; i < stockUI.Length; i++)
        {
            if (stock[i] != null)
            {
                stockUI[i].gameObject.SetActive(true);
                stockUI[i].Fill(stock[i]);
            }
            else
            {
                stockUI[i].gameObject.SetActive(false);
            }
            if (i != stockUI.Length - 1 && aux[i] != null)
            {
                inventoryUI[i].gameObject.SetActive(true);
                SellItem shopItem = new SellItem(DataBase.GetItem(aux[i].name), aux[i].inventoryAmount);
                inventoryUI[i].Fill(shopItem);
            }
            else
            {
                inventoryUI[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary> 
    /// Presionas un boton del inventario para meterlo a la tienda 
    /// </summary> 
    /// <param name="pos"></param> 
    public void Button(int pos)
    {
        position = pos;
    }

    /// <summary> 
    /// Añades un Item del inventario al array  
    /// </summary> 
    /// <param name="amount"></param> 
    public void AddItem(int amount)
    {
        string id = InventoryController.Instance.GetID(position);
        int currentPosition = SearchStock(id);

        //Añadir a stock 
        if (currentPosition >= 0)
        {
            stock[currentPosition].amount += amount;
        }
        else
        {
            UpdateBoxEnable(id);
            stock[numStock] = new SellItem(DataBase.GetItem(id), amount);
            numStock++;
        }
        InventoryController.Instance.SubstractAmountItem(amount, id);
        ShowStock();
    }

    /// <summary> 
    /// buscas la posicion de un objeto concreto en el array (devuelve la posicion o -1) 
    /// </summary> 
    /// <param name="name"></param> 
    /// <returns></returns> 
    private int SearchStock(string name)
    {
        for (int i = 0; i < stock.Length; i++)
        {
            if (stock[i] != null && stock[i].item.name == name)
            {
                return i;
            }
        }
        return -1;
    }

    private void UpdateBoxEnable(string name)
    {
        bool end = false;
        Transform aux = transform.GetChild(0);
        for (int i = 0; i < aux.childCount && !end; i++)
        {
            if (aux.GetChild(i).name == name)
            {
                end = true;
                aux.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    private void UpdateBoxDisable(string name)
    {
        bool end = false;
        Transform aux = transform.GetChild(0);
        for (int i = 0; i < aux.childCount && !end; i++)
        {
            if (aux.GetChild(i).name == name)
            {
                end = true;
                aux.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary> 
    /// El coche viene y se leva un item 
    /// </summary> 
    public void SellItem()
    {
        if (numStock > 0)
        {
            int pos = Random.Range(0, numStock);
            SellItem item = stock[pos];
            int cant = Random.Range(item.amount / 4, item.amount);

            AddMoney(item.item.price * cant);

            if (cant == item.amount)
            {
                stock[pos] = null;
                ReOrder();
                numStock--;

                transform.GetChild(0).GetChild(numStock).gameObject.SetActive(false);
            }
            else
            {
                item.amount -= cant;
            }
        }

    }

    IEnumerator UiIconOff()
    {
        yield return new WaitForSeconds(3f);
        UiIcon.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            outline.ShowOutline();
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            outline.HideOutline();
        }
            
    }
}