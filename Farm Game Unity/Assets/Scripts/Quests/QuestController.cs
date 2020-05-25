using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    public static QuestController Instance;

    private bool playerIn;
    public Transform questPanelFolder;
    private GameObject[] questPanels;

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

        int max = questPanelFolder.childCount;
        questPanels = new GameObject[max];
        for (int i = 0; i < max; i++)
        {
            questPanels[i] = questPanelFolder.GetChild(i).gameObject;
        }
    }
    private void Update()
    {
        if (playerIn)
        {
            if (Input.GetKeyDown(InputManager.instance.Interact))
            {
                UpdatePanels();
                InventoryController.Instance.OpenMenu();
                InventoryController.Instance.ChangePage(2);
            }
        }
    }

    public void UpdatePanels()
    {
        string[,] descriptions = GetQuestsDescriptions();
        int index = 0;
        for (int i = 0; i < completedQuests.Count; i++)
        {
            questPanels[index].SetActive(true);
            questPanels[index].GetComponent<QuestEntry>().Fill(descriptions[0, index], descriptions[1, index]);
            questPanels[index].GetComponent<QuestEntry>().AssignQuest(completedQuests[i]);
            index++;
        }
        for (int i = 0; i < activeQuests.Count; i++)
        {
            questPanels[index].SetActive(true);
            questPanels[index].GetComponent<QuestEntry>().Fill(descriptions[0, index], descriptions[1, index]);
            questPanels[index].GetComponent<QuestEntry>().AssignQuest(activeQuests[i]);
            index++;
        }
        for (int i = index; i < questPanels.Length; i++)
        {
            questPanels[index].SetActive(false);
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

    private string[,] GetQuestsDescriptions()
    {
        string[,] description = new string[2, completedQuests.Count + activeQuests.Count];

        int index = 0;
        BuildDescription(description, completedQuests, index);
        BuildDescription(description, activeQuests, index);

        return description;
    }

    private void BuildDescription(string[,] description, List<Quest> list, int index)
    {
        for (int i = 0; i < list.Count; i++)
        {
            description[0, index] = list[i].QuestName;

            description[1, index] = "";
            description[1, index] += list[i].QuestName + " " + list[i].Description + " " + list[i].NPCName;
            description[1, index] += "\n";
            description[1, index] += list[i].ItemReward.itemName;

            for (int j = 0; j < list[i].Goals.Count; j++)
            {
                Goal g = list[i].Goals[j];
                description[1, index] += "\n";
                description[1, index] += "- " + g.CurrentAmount + " / " + g.Description;
            }
            index++;
        }
    }

    public void AddUpdatedQuest(Quest q)
    {
        completedQuests.Remove(q);
        activeQuests.Add(q);
    }
    public void RemoveQuest(Quest q)
    {
        if(completedQuests.Contains(q))
        {
            completedQuests.Remove(q);
            UpdatePanels();
        }
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
