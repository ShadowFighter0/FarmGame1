
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest
{
    public List<Goal> Goals { get; set; }
    public string QuestName { get; set; }
    public string Description { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }
    public string NPCName { get; set; }
    public bool ItemGiven { get; set; }
    public int QuestExp { get; set; }

    public bool IsOrder { get; set; }

    public bool CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        if(Completed)
        {
            QuestController.Instance.CheckQuest(this);
        }
        return Completed;
    }

    public void GiveReward()
    {
        if (Completed)
        {
            if (ItemReward != null)
            {
                ItemGiven = true;
                foreach (Goal g in Goals)
                {
                    InventoryController.Instance.SubstractAmountItem(g.RequiredAmount, g.ItemID);
                }
                InventoryController.Instance.AddItem(ItemReward);
                QuestController.Instance.RemoveQuest(this);
            }
            if (QuestExp > 0)
            {
                PlayerManager.instace.AddExp(QuestExp);
            }
        }
    }
}