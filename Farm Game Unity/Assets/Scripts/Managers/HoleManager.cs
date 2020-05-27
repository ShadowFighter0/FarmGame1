using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public float time;
    public WorldItem(Vector3 pos, string name, PlantInfo info, float w, float t)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        prefab = name;
        plantInfo = info;
        water = w;
        time = t;
    }
}
public class HoleManager : MonoBehaviour
{
    public static HoleManager instance;
    private void Awake()
    {
        GameEvents.OnSaveInitiated += SaveHoles;
        if (instance != null && instance != this)
        {
            Debug.Log("deleting double singleton");
            GameEvents.OnSaveInitiated -= SaveHoles;
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        if (SaveLoad.SaveExists("Holes"))
        {
            List<WorldItem> holesSaveds = SaveLoad.Load<List<WorldItem>>("Holes");
            foreach (WorldItem info in holesSaveds)
            {
                Vector3 pos = new Vector3(info.x, info.y, info.z);
                GameObject hole = ObjectPooler.Instance.SpawnFromPool(info.prefab, pos, Quaternion.identity);

                HoleController holeScript = hole.GetComponent<HoleController>();
                holeScript.SetWater(info.water);
                holeScript.Timer = info.time;
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
                    script.InitializePlant(plant.index, s);
                    script.SetScript(holeScript);

                    go.transform.SetParent(hole.transform);
                }
            }
        }
    }

    private void SaveHoles()
    {
        if(transform.childCount > 0)
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
                float time = child.GetComponent<HoleController>().Timer;
                WorldItem childInfo = new WorldItem(child.position, "Holes", plantInfo, water, time);
                infos.Add(childInfo);
            }
            SaveLoad.Save(infos, "Holes");
        }
    }
}
