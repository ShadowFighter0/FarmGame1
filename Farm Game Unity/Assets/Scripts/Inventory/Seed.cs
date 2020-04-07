using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Seed")]
public class Seed : Item
{
    public GameObject plantType;
    public Item food;
    public int growthTime;
}
