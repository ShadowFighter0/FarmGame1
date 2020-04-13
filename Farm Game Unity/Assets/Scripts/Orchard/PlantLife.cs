using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlantInfo
{
    public int index;
    public string seed;
    public string plant;
    public int currentGrowthTime;
    public float x;
    public float y;
    public float z;

    public PlantInfo (int i, string s, string p, int time, Vector3 pos)
    {
        index = i;
        seed = s;
        plant = p;
        currentGrowthTime = time;
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
}
public class PlantLife : MonoBehaviour
{
    private int index = 0;
    private HoleController holeScript;

    private Seed seed;
    private int currentGrowthTime;
    private bool grownUp = false;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Plants");
        ChangeModel();
        holeScript = gameObject.GetComponentInParent<HoleController>();
        currentGrowthTime = seed.growthTime;
    }

    public int GetFood()
    {
        return seed.food.amount; 
    }
    public PlantInfo SavePlant()
    {
        return new PlantInfo(index, seed.itemName, seed.plantType, currentGrowthTime, transform.position);
    }
    public void InitializePlant(int i, Seed s, int time)
    {
        index = i;
        SetSeed(s);
        currentGrowthTime = time;
    }
    public void SetSeed(Seed s) { seed = s; }

    public bool GetGrownUp() { return grownUp; }

    public void NewDay()
    {
        if (holeScript.GetWet() && index < transform.childCount-1)
        {
            currentGrowthTime--;
            if(currentGrowthTime <= 0)
            {
                currentGrowthTime = seed.growthTime;
                index++;
                ChangeModel();
            }
        }
        if (!holeScript.GetWet())
        {
            Destroy(gameObject);
        }   
    }

    private void ChangeModel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (index >= transform.childCount - 1)
        {
            grownUp = true;
        }
    }

    public void AddInventory()
    { 
        InventoryController.Instance.AddItem(seed.food);
        InventoryController.Instance.AddItem(seed);
    }
}
