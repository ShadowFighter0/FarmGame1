using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest")]
public class QuestTemplate : ScriptableObject
{
    public string title;
    public string description;
    public string NPCName;
    public Item itemReward;
    [Header("Goals")]
    public string[] ids;
    public int[] amounts;
}
