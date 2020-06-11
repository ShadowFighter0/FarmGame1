using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quest")]
public class QuestTemplate : ScriptableObject
{
    public string title;
    [TextArea]
    public string description;
    public string NPCName;
    public int experience;
    [Header("Goals")]
    public string[] ids;
    public int[] amounts;
    public bool isOrder;
}
