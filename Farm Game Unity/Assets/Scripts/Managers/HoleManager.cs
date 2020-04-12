using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldItem
{
    public float x;
    public float y;
    public float z;
    public string prefab;
    public WorldItem(Vector3 pos, string name)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        prefab = name;
    }
}
public class HoleManager : MonoBehaviour
{
    public static HoleManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameEvents.Instance.OnSaveInitiated += SaveHoles;

        if (SaveLoad.SaveExists("Holes"))
        {
            List<WorldItem> holesSaveds = new List<WorldItem>();
            holesSaveds = SaveLoad.Load<List<WorldItem>>("Holes");
            foreach (WorldItem info in holesSaveds)
            {
                Vector3 pos = new Vector3(info.x, info.y, info.z);
                GameObject hole = ObjectPooler.Instance.SpawnFromPool(info.prefab, pos, Quaternion.identity);
                hole.transform.SetParent(transform);
            }
        }
    }

    public void SaveHoles()
    {
        List<WorldItem> infos = new List<WorldItem>();
        foreach (Transform child in transform)
        {
            WorldItem childInfo = new WorldItem(child.position, "Holes");
            infos.Add(childInfo);
        }
        SaveLoad.Save(infos, "Holes");
    }
}
