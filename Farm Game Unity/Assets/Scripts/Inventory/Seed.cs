﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Seed")]
[System.Serializable]
public class Seed : Item
{
    public Item food;
    public int growthTime;
    public int lvl;
}
