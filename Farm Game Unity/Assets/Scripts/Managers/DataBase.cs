using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private static Item[] items;
    private static GameObject[] plantPrefabs;

    private static AudioClip[] audioClips;
    private void Awake()
    {
        items = Resources.LoadAll<Item>("Data/Items");
        plantPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        audioClips = Resources.LoadAll<AudioClip>("AudioClips");
    }

    public static Item GetItem(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName.Equals(name))
            {
                return items[i];
            }
        }
        return null;
    }
    public static Sprite GetItemSprite(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName.Equals(name))
            {
                return items[i].image;
            }
        }
        return null;
    }
    public static GameObject SearchPrefab(string name)
    {
        for (int i = 0; i < plantPrefabs.Length; i++)
        {
            if(plantPrefabs[i].name.Equals(name))
            {
                return plantPrefabs[i];
            }
        }

        return null;
    }

    public static GameObject PlantPrefab(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetType().Equals(typeof(Seed)))
            {
                Seed seed = (Seed)items[i];
                if(seed.food.itemName.Equals(name))
                {
                    return SearchPrefab(name);
                }
            }
        }
        return null;
    }

    public static AudioClip SearchClip(string name)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if(audioClips[i].name.Equals(name))
            {
                return audioClips[i];
            }
        }
        return null;
    }
}
