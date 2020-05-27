using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleQuest : Quest
{
    public SampleQuest(string questName, string npcName, string description, string[] itemId, int[] itemAmount, Item reward, int exp, bool isOrd)
    {
        QuestName = questName;
        Description = description;
        NPCName = npcName;
        ItemReward = reward;
        NpcManager.instance.AddQuest(this, NPCName);
        Goals = new List<Goal>();
        for (int i = 0; i < itemId.Length; i++)
        {
           Goals.Add(new CollectionGoal(this, itemId[i], false, InventoryController.Instance.GetAmount(itemId[i]), itemAmount[i]));
        }
        Completed = false;
        QuestExp = exp;
        IsOrder = isOrd;
        Goals.ForEach(g => g.Init());
        CheckGoals();
    }
}
