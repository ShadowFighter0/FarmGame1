using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    private Seed[] seeds;
    private List<Seed> currentSeeds = new List<Seed>();
    
    public GameObject indicator;
    private SeedsIndicator indicatorScript;

    private int index = 0;

    public int Index
    {
        get => index; 
        set 
        {
            int maxSeeds = currentSeeds.Count - 1;
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
        seeds = Resources.LoadAll<Seed>("Data/Items");
    }
    private void Start()
    {
        indicatorScript = indicator.GetComponent<SeedsIndicator>();

        UpdateCurrentSeeds();
    }

    public int GetCurrentSeeds() { return currentSeeds.Count; }
    public List<Seed> CurrentSeeds() { return currentSeeds; }
    public void UpdateCurrentSeeds()
    {
        List<Seed> seedsToRemove = new List<Seed>();
        foreach (Seed s in seeds)
        {
            if (InventoryController.Instance.GetAmount(s.itemName) > 0)
            {
                if(!currentSeeds.Contains(s))
                {
                    currentSeeds.Add(s);
                    UpdateIndicator();
                }
            }
            else if (currentSeeds.Contains(s))
            {
                seedsToRemove.Add(s);
            }
        }
        foreach (Seed s in seedsToRemove)
        {
            currentSeeds.Remove(s);
            if(Index == currentSeeds.Count && currentSeeds.Count > 0)
            {
                Index--;
            }
        }
        if (currentSeeds.Count > 0)
        {
            UpdateIndicator();
        }
        seedsToRemove.Clear();
    }

    void Update()
    {
        if(currentSeeds.Count > 0)
        {
            CheckTarget();
        }
        else if (SeedsIndicator.instance.gameObject.activeSelf)
        {
            SeedsIndicator.instance.gameObject.SetActive(false);
        }
    }
    public void UpdateIndicator()
    {
        indicatorScript.ActivateChild(currentSeeds[Index].food.itemName);
    }

    private void CheckTarget()
    {
        GameObject go = RayCastController.instance.GetTarget();
        if (go != null)
        {
            if (go.CompareTag("Hole"))
            {
                if (!go.GetComponent<HoleController>().HasPlant())
                {
                    indicator.gameObject.SetActive(true);
                }
                else if (indicator.activeSelf)
                {
                    indicator.gameObject.SetActive(false);
                }

                indicatorScript.ChangePosition(go.transform.position);

                if (Input.GetKeyDown(InputManager.instance.Interact) && InventoryController.Instance.GetAmount(currentSeeds[Index].itemName) > 0 && InputManager.state == InputManager.States.Idle)
                {
                    if (go.transform.childCount < 1)
                    {
                        InputManager.instance.playerAnim.SetTrigger("Plant");
                        StartCoroutine(AnimDelay(go));
                    }
                }
            }
            else if (indicator.activeSelf)
            {
                indicator.SetActive(false);
            }
        }
    }
    IEnumerator AnimDelay(GameObject go)
    {
        yield return new WaitForSeconds(0.3f);
        Plant(go);
    }

    private void Plant(GameObject go)
    {
        GameObject loadPlant = DataBase.GetPlantPrefab(currentSeeds[Index].food.itemName);

        Vector3 rot = new Vector3(0.0f, Random.Range(1, 360), 0.0f);
        GameObject plant = Instantiate(loadPlant, go.transform.position, Quaternion.Euler(rot));

        plant.transform.SetParent(go.transform);
        
        plant.GetComponent<PlantLife>().SetSeed(currentSeeds[Index]);
        plant.GetComponent<PlantLife>().SetScript(go.GetComponent<HoleController>());

        InventoryController.Instance.SubstractAmountSeed(1, currentSeeds[Index].itemName);
        UpdateCurrentSeeds();
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
}
