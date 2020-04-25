using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sell : MonoBehaviour
{
    ShopItem[] stock;
    public bool playerNear;
    
    
    public static Sell Instance;


    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(int pos)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }

}
