using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private static readonly Dictionary<string, Item> items = new Dictionary<string, Item>();
    private static readonly Dictionary<string, GameObject> plantPrefabs = new Dictionary<string, GameObject>();
    private static readonly Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
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
    }
    public static Item GetItem(string name) { return items[name]; }
    public static Sprite GetItemSprite(string name) { return items[name].image; }
    public static GameObject GetPlantPrefab(string name) { return plantPrefabs[name]; }
    public static AudioClip GetAudioClip(string name) { return audioClips[name]; }
}
