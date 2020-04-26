using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string ItemID { get; set; }
    public Quest Quest { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public virtual void Init()
    {
        //default init stuff
    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
        else
        {
            Completed = false;
        }
    }

    public void Complete()
    {
        Quest.CheckGoals();
        Completed = true;
    }
}