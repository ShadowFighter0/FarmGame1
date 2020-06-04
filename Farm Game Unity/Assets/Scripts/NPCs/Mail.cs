using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Mail")]
public class Mail : ScriptableObject
{
    [TextArea]
    public string message;
    public Item rewardItem;
    public int amount;
    public QuestTemplate quest;
}
