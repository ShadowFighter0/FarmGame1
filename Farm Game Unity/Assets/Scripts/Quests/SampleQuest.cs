using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleQuest : Quest
{
    public SampleQuest(string questName, string npcName, string description, string[] itemId, int[] itemAmount, Item reward)
    {
        Debug.Log(questName + " assigned.");
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
        Goals.ForEach(g => g.Init());
        CheckGoals();
    }
}
