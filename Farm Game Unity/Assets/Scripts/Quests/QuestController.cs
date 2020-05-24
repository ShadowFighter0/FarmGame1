using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveQuest
{
    public string questName;
    public string npcName;
    public string description;
    public string[] itemId;
    public int[] itemAmount;
    public string reward;
    public SaveQuest(string q, string npc, string d, string[] ids, int[] am, string r)
    {
        questName = q;
        npcName = npc;
        description = d;
        itemId = ids;
        itemAmount = am;
        reward = r;
    }
}

public class QuestController : MonoBehaviour
{
    List<Quest> activeQuests = new List<Quest>();
    List<Quest> completedQuests = new List<Quest>();
    public static QuestController Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameEvents.OnSaveInitiated += SaveQuests;

        if(SaveLoad.SaveExists("ActiveQuests"))
        {
            List<SaveQuest> saved = SaveLoad.Load<List<SaveQuest>>("ActiveQuests");
            foreach (SaveQuest q in saved)
            {
                Item item = DataBase.GetItem(q.reward);
                AddQuest(new SampleQuest(q.questName, q.npcName, q.description, q.itemId, q.itemAmount, item));
            }
            saved.Clear();
        }
        if (SaveLoad.SaveExists("CompletedQuests"))
        {
            List<SaveQuest> saved = SaveLoad.Load<List<SaveQuest>>("CompletedQuests");
            foreach (SaveQuest q in saved)
            {
                Item item = DataBase.GetItem(q.reward);
                AddQuest(new SampleQuest(q.questName, q.npcName, q.description, q.itemId, q.itemAmount, item));
            }
            saved.Clear();
        }
    }

    private void SaveQuests()
    {
        List<SaveQuest> saveActives = new List<SaveQuest>();
        foreach (Quest q in activeQuests)
        {
            string [] ids = new string[q.Goals.Count];
            int [] amounts = new int[q.Goals.Count];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = q.Goals[i].ItemID;
                amounts[i] = q.Goals[i].RequiredAmount;
            }
            saveActives.Add(new SaveQuest(q.QuestName, q.NPCName, q.Description, ids, amounts, q.ItemReward.itemName));
        }
        List<SaveQuest> saveCompleted = new List<SaveQuest>();
        foreach (Quest q in completedQuests)
        {
            string[] ids = new string[q.Goals.Count];
            int[] amounts = new int[q.Goals.Count];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = q.Goals[i].ItemID;
                amounts[i] = q.Goals[i].RequiredAmount;
            }
            saveCompleted.Add(new SaveQuest(q.QuestName, q.NPCName, q.Description, ids, amounts, q.ItemReward.itemName));
        }

        SaveLoad.Save(saveActives, "ActiveQuests");
        SaveLoad.Save(saveCompleted, "CompletedQuests");

        saveActives.Clear();
        saveCompleted.Clear();
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
