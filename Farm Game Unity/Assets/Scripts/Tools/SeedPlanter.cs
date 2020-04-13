using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    public Seed[] seeds;

    private int index = 0;
    public int Index
    {
        get => index; 
        set 
        {
            int maxSeeds = seeds.Length - 1;
            if(value > maxSeeds)
            {
                index = maxSeeds;
            }
            else if(value < 0)
            {
                index = 0;
            }
            else
            {
                index = value;
            }
        }
    }

    public static SeedPlanter instance;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (InputManager.state == InputManager.States.Working)
        {
            CheckTarget();
        }
    }

    private void CheckTarget()
    {
        GameObject go = RayCastController.instance.GetTarget();
        if (go != null)
        {
            if (go.CompareTag("Hole"))
            {
                ActionTextController.instance.ChangePosition(go.transform.position);
                ActionTextController.instance.ChangeText("Press E to plant: " + seeds[index].food.itemName);

                if (Input.GetKeyDown(InputManager.instance.Click) && InventoryController.Instance.GetAmount(seeds[index].itemName) > 0)
                {
                    Plant(go);
                }
            }
        }
    }

    private void Plant(GameObject go)
    {
        if (go.transform.childCount < 1)
        {
            GameObject loadPlant = Resources.Load<GameObject>("Prefabs/" + seeds[index].plantType);
            GameObject plant = Instantiate(loadPlant, go.transform.position, Quaternion.identity);
            plant.transform.SetParent(go.transform);
            plant.GetComponent<PlantLife>().SetSeed(seeds[index]);

            InventoryController.Instance.SubstractAmountSeed(1, seeds[index].itemName);
        }
    }
    public Seed GetSeed(string name)
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if(seeds[i].itemName.Equals(name))
            {
                return seeds[i];
            }
        }
        return null;
    }
    public int GetSeedsLenght() { return seeds.Length; }
    public void SetSeed(int i)
    {
        Index = i;
    }
}
