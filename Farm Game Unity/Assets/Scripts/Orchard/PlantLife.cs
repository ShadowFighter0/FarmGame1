using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLife : MonoBehaviour
{
    private int index = 0;
    private HoleController holeScript;

    private Seed seed;
    private int growthTime;
    private int currentGrowthTime;
    private bool grownUp = false;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Plants");
        growthTime = seed.growthTime;
        ChangeModel();
        holeScript = gameObject.GetComponentInParent<HoleController>();
        currentGrowthTime = growthTime;
    }

    public int GetFood()
    {
        return seed.food.amount; 
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
                currentGrowthTime = growthTime;
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
        //InventoryController.Instance.AddItem(seed);
    }
}
