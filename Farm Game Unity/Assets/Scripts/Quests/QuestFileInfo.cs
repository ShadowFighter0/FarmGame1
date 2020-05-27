using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestFileInfo : MonoBehaviour
{
    private readonly Queue<QuestInfo> quests = new Queue<QuestInfo>();
    private QuestTemplate[] questsInfos;
    public static QuestFileInfo Instance;
    private void Awake()
    {
        Instance = this;
        FillQueue();
    }
    private void FillQueue()
    {
        questsInfos = Resources.LoadAll<QuestTemplate>("Data/Quests");
        for (int i = 0; i < questsInfos.Length; i++)
        {
            quests.Enqueue(new QuestInfo(questsInfos[i].title, questsInfos[i].description, questsInfos[i].ids, questsInfos[i].amounts, questsInfos[i].NPCName, questsInfos[i].itemReward, questsInfos[i].experience, questsInfos[i].isOrder));
        }
    }

    public QuestInfo GetQuest()
    {
        return quests.Dequeue();
    }
}

public class QuestInfo
{
    public string title;
    public string description;
    public string[] ids;
    public int[] amounts;
    public string npcName;
    public Item itemReward;
    public int experience;
    public bool isOrder;
    public QuestInfo(string s, string d, string[] id, int[] am, string npc, Item item, int exp, bool isOrd)
    {
        title = s;
        description = d;
        ids = id;
        amounts = am;
        npcName = npc;
        itemReward = item;
        experience = exp;
        isOrder = isOrd;
    }
}
