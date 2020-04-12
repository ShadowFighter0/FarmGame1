using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string itemName;
    public string imagePath;

    public int amount = 0;

    public int price = 0;

    public void SetAmount(int am)
    {
        amount = am;
    }
}
