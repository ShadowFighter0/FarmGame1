using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private static readonly Dictionary<string, Item> items = new Dictionary<string, Item>();
    private static readonly Dictionary<string, GameObject> plantPrefabs = new Dictionary<string, GameObject>();
    private static readonly Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private static readonly Dictionary<int, QuestInfo> quests = new Dictionary<int, QuestInfo>();

    private void Awake()
    {
        items.Clear();
        plantPrefabs.Clear();
        audioClips.Clear();
        quests.Clear();
        InitDictionarys();
    }
    private static void InitDictionarys()
    {
        Item[] loadItems = Resources.LoadAll<Item>("Data/Items");
        for (int i = 0; i < loadItems.Length; i++)
        {
            items.Add(loadItems[i].itemName, loadItems[i]);
        }

        GameObject[] loadPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        for (int i = 0; i < loadPrefabs.Length; i++)
        {
            plantPrefabs.Add(loadPrefabs[i].name, loadPrefabs[i]);
        }

        AudioClip[] clips = Resources.LoadAll<AudioClip>("AudioClips");
        for (int i = 0; i < clips.Length; i++)
        {
            audioClips.Add(clips[i].name, clips[i]);
        }

        QuestTemplate[] questsInfos = Resources.LoadAll<QuestTemplate>("Data/Quests");
        for (int i = 0; i < questsInfos.Length; i++)
        {
            quests.Add(i,new QuestInfo(questsInfos[i]));
        }
    }
    public static Item GetItem(string name) { return items[name]; }
    public static Sprite GetItemSprite(string name) { return items[name].image; }
    public static GameObject GetPlantPrefab(string name) { return plantPrefabs[name]; }
    public static AudioClip GetAudioClip(string name) { return audioClips[name]; }
    public static QuestInfo GetQuest(int i) { return quests[i]; }
}

public class QuestInfo
{
    public string questName;
    public string description;
    public string[] ids;
    public int[] amounts;
    public string npcName;
    public Item itemReward;
    public int amount;
    public int experience;
    public bool isOrder;
    public QuestInfo(QuestTemplate q)
    {
        questName = q.title;
        description = q.description;
        ids = q.ids;
        amounts = q.amounts;
        npcName = q.NPCName;
        itemReward = q.itemReward;
        experience = q.experience;
        isOrder = q.isOrder;
        amount = q.amount;
    }
}
