using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveQuest
{
    public string questName;
    public string npcName;
    public string description;
    public string[] itemId;
    public int[] itemAmount;
    public string reward;
    public int amount;
    public int experience;
    public bool isOrder;
    public SaveQuest(string q, string npc, string d, string[] ids, int[] am, string r, int amo, int exp, bool isOrd)
    {
        questName = q;
        npcName = npc;
        description = d;
        itemId = ids;
        itemAmount = am;
        reward = r;
        experience = exp;
        isOrder = isOrd;
        amount = amo;
    }
}

public class QuestController : MonoBehaviour
{
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    public static QuestController Instance;
    private bool playerIn = false;
    public Transform questPanelFolder;
    private GameObject[] questPanels;
    private bool tutorialDone;
    private void Awake()
    {
        Instance = this;
        GameEvents.OnTutorialDone += TutorialDone;
    }

    private void TutorialDone()
    {
        tutorialDone = true;
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
                AddQuest(new SampleQuest(q));
            }
            saved.Clear();
        }
        if (SaveLoad.SaveExists("CompletedQuests"))
        {
            List<SaveQuest> saved = SaveLoad.Load<List<SaveQuest>>("CompletedQuests");
            foreach (SaveQuest q in saved)
            {
                Item item = DataBase.GetItem(q.reward);
                AddQuest(new SampleQuest(q));
            }
            saved.Clear();
        }

        int max = questPanelFolder.childCount;
        questPanels = new GameObject[max];
        for (int i = 0; i < max; i++)
        {
            questPanels[i] = questPanelFolder.GetChild(i).gameObject;
            questPanels[i].SetActive(true);
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
            questPanels[index].GetComponent<QuestEntry>().Fill(descriptions[0, index], descriptions[1, index]);
            questPanels[index].GetComponent<QuestEntry>().AssignQuest(completedQuests[i]);
            index++;
        }
        for (int i = 0; i < activeQuests.Count; i++)
        {
            questPanels[index].GetComponent<QuestEntry>().Fill(descriptions[0, index], descriptions[1, index]);
            questPanels[index].GetComponent<QuestEntry>().AssignQuest(activeQuests[i]);
            index++;
        }
        for (int i = index; i < questPanels.Length; i++)
        {
            questPanels[index].GetComponent<QuestEntry>().Fill();
            questPanels[index].GetComponent<QuestEntry>().AssignQuest(null);
            index++;
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
            saveActives.Add(new SaveQuest(q.QuestName, q.NPCName, q.Description, ids, amounts, q.ItemReward.itemName, q.Amount, q.QuestExp, q.IsOrder));
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
            saveCompleted.Add(new SaveQuest(q.QuestName, q.NPCName, q.Description, ids, amounts, q.ItemReward.itemName, q.Amount, q.QuestExp, q.IsOrder));
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
        BuildDescription(ref description, completedQuests, ref index);
        BuildDescription(ref description, activeQuests, ref index);

        return description;
    }

    private void BuildDescription(ref string[,] description, List<Quest> list, ref int index)
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

            if (!tutorialDone)
            {
                TutorialController.instance.SetThingDone(2);
            }
        }
    }

    public void OnRemoveQuestButton(Quest q)
    {
        if(completedQuests.Contains(q) && q.IsOrder)
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

    #region Trigger with player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = false;
        }
    }
    #endregion
}
