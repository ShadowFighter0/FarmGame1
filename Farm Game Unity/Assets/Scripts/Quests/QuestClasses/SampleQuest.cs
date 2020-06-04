using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleQuest : Quest
{
    public SampleQuest(QuestInfo q)
    {
        QuestName = q.questName;
        Description = q.description;
        NPCName = q.npcName;
        ItemReward = q.itemReward;
        Amount = q.amount;
        NpcManager.instance.AddQuest(this, NPCName);
        Goals = new List<Goal>();
        for (int i = 0; i < q.ids.Length; i++)
        {
           Goals.Add(new CollectionGoal(this, q.ids[i], false, InventoryController.Instance.GetAmount(q.ids[i]), q.amounts[i]));
        }
        Completed = false;
        QuestExp = q.experience;
        IsOrder = q.isOrder;
        Goals.ForEach(g => g.Init());
        CheckGoals();
    }
    public SampleQuest(SaveQuest q)
    {
        QuestName = q.questName;
        Description = q.description;
        NPCName = q.npcName;
        ItemReward = DataBase.GetItem(q.reward);
        Amount = q.amount;
        NpcManager.instance.AddQuest(this, NPCName);
        Goals = new List<Goal>();
        for (int i = 0; i < q.itemId.Length; i++)
        {
           Goals.Add(new CollectionGoal(this, q.itemId[i], false, InventoryController.Instance.GetAmount(q.itemId[i]), q.itemAmount[i]));
        }
        Completed = false;
        QuestExp = q.experience;
        IsOrder = q.isOrder;
        Goals.ForEach(g => g.Init());
        CheckGoals();
    }
}
