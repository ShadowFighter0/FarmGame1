using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/UnlockeableItem")]
[System.Serializable]
public class UnlockeableItem : MonoBehaviour
{
    public string itemName;
    public string description;
    public Item[] requirements;
    public int[] amounts;
}
