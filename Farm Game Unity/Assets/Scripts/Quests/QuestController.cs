using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    List<Quest> activeQuests = new List<Quest>();
    List<Quest> completedQuests = new List<Quest>();
    public static QuestController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void AddQuest(Quest q)
    {
        activeQuests.Add(q);
        q.CheckGoals();
    }

    private void OnGUI()
    {
        int pos = 20;
        GUI.Label(new Rect(10, pos, 300, 300), "ACTIVE");
        pos += 20;
        for (int i = 0; i < activeQuests.Count; i++)
        {
            GUI.Label(new Rect(20, pos, 300, 300), activeQuests[i].QuestName + ": " + activeQuests[i].Description + " " + activeQuests[i].NPCName);
            pos += 15;
            GUI.Label(new Rect(20, pos, 300, 300), activeQuests[i].ItemReward.itemName);
            pos += 15;
            for (int j = 0; j < activeQuests[i].Goals.Count; j++)
            {
                Goal g = activeQuests[i].Goals[j];
                string s = "- " + g.CurrentAmount + " / " + g.Description;
                GUI.Label(new Rect(20, pos, 300, 300), s);
                pos += 15;
            }
        }
        pos += 15;
        GUI.Label(new Rect(10, pos, 300, 300), "COMPLETED");
        pos += 20;
        for (int i = 0; i < completedQuests.Count; i++)
        {
            GUI.Label(new Rect(20, pos, 300, 300), completedQuests[i].QuestName + " " + completedQuests[i].Description + " " + completedQuests[i].NPCName);
            pos += 15;
            GUI.Label(new Rect(20, pos, 300, 300), completedQuests[i].ItemReward.itemName);
            pos += 15;
            
            for (int j = 0; j < completedQuests[i].Goals.Count; j++)
            {
                Goal g = completedQuests[i].Goals[j];
                string s = "- " + g.CurrentAmount + " / " + g.Description;
                GUI.Label(new Rect(20, pos, 300, 300), s);
                pos += 15;
            }
        }
    }

    public void AddUpdatedQuest(Quest q)
    {
        completedQuests.Remove(q);
        activeQuests.Add(q);
    }
    public void RemoveQuest(Quest q)
    {
        completedQuests.Remove(q);
    }
    public void AddCompletedQuest(Quest q)
    {
        completedQuests.Add(q);
        activeQuests.Remove(q);
    }
    public void CheckQuest(Quest q)
    {
        if(q != null && q.Completed && activeQuests.Contains(q))
        {
            Debug.Log(q.QuestName + " completed!");
            AddCompletedQuest(q);
        }
    }
}
