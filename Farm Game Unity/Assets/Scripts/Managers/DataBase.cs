using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private static Item[] items;
    private void Awake()
    {
        items = Resources.LoadAll<Item>("Data/Items");
    }

    public static Item GetItem(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName.Equals(name))
            {
                return items[i];
            }
        }
        return null;
    }

    public static Sprite GetItemSprite(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName.Equals(name))
            {
                return items[i].image;
            }
        }
        return null;
    }
}
