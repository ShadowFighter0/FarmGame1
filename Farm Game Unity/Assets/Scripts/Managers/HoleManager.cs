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
    public PlantInfo plantInfo;
    public float water;
    public WorldItem(Vector3 pos, string name, PlantInfo info, float w)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        prefab = name;
        plantInfo = info;
        water = w;
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
                hole.GetComponent<HoleController>().SetWater(info.water);
                hole.transform.SetParent(transform);

                PlantInfo plant = info.plantInfo;
                if (plant != null)
                {
                    GameObject plantLoad = Resources.Load<GameObject>("Prefabs/" + plant.plant);
                    GameObject go = Instantiate(plantLoad);

                    pos = new Vector3(plant.x, plant.y, plant.z);
                    go.transform.position = pos;

                    PlantLife script = go.GetComponent<PlantLife>();
                    Seed s = SeedPlanter.instance.GetSeed(plant.seed);
                    script.InitializePlant(plant.index, s, plant.currentGrowthTime);

                    go.transform.SetParent(hole.transform);
                }
            }
        }
    }

    public void SaveHoles()
    {
        List<WorldItem> infos = new List<WorldItem>();
        foreach (Transform child in transform)
        {
            PlantInfo plantInfo = null;
            if (child.childCount > 0)
            {
                plantInfo = child.GetChild(0).GetComponent<PlantLife>().SavePlant();
            }
            float water = child.GetComponent<HoleController>().GetWater();
            WorldItem childInfo = new WorldItem(child.position, "Holes", plantInfo, water);
            infos.Add(childInfo);
        }
        SaveLoad.Save(infos, "Holes");
    }
}
