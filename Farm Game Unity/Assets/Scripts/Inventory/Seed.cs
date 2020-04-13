﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Seed")]
[System.Serializable]
public class Seed : Item
{
    public string plantType;
    public Item food;
    public int growthTime;
}
