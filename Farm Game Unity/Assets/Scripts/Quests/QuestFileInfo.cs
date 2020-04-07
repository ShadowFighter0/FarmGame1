using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestFileInfo : MonoBehaviour
{
    private Queue<QuestInfo> quests = new Queue<QuestInfo>();
    public QuestTemplate []questsInfos;
    public static QuestFileInfo Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FillQueue();
    }

    private void FillQueue()
    {
        for (int i = 0; i < questsInfos.Length; i++)
        {
            quests.Enqueue(new QuestInfo(questsInfos[i].title, questsInfos[i].description, questsInfos[i].ids, questsInfos[i].amounts, questsInfos[i].NPCName, questsInfos[i].itemReward));
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

    public QuestInfo(string s, string d, string[] id, int[] am, string npc, Item item)
    {
        title = s;
        description = d;
        ids = id;
        amounts = am;
        npcName = npc;
        itemReward = item;
    }
}
