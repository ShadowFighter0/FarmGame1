using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Mail")]
public class Mail : ScriptableObject
{
    public string message;
    public Item rewardItem;
    public Quest quest;
}
