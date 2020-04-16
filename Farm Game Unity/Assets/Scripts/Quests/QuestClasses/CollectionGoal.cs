using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{
    public CollectionGoal(Quest quest, string itemID, bool completed, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        ItemID = itemID;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
        Description = requiredAmount + " " + itemID;
        Completed = completed;
        Evaluate();
        Quest.CheckGoals();
    }

    public override void Init()
    {
        base.Init();
        GameEvents.OnItemCollected += ItemPickedUp;
    }

    void ItemPickedUp(string s, int am)
    {
        if (s.Equals(ItemID) && !Quest.itemGiven)
        {
            bool currentState = Quest.Completed;
            CurrentAmount = am;
            Evaluate();
            Quest.CheckGoals();
            if(currentState == true && Quest.Completed == false)
            {
                QuestController.Instance.AddUpdatedQuest(Quest);
            }
        }
    }
}