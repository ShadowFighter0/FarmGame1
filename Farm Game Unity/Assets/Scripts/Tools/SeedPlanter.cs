using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    public Seed[] seeds;
    private List<Seed> currentSeeds = new List<Seed>();
    private Animator anim;

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
        GameEvents.OnAnimatorSelected += SetAnimator;
    }
    private void Start()
    {
        UpdateCurrentSeeds();
    }

    public void UpdateCurrentSeeds()
    {
        foreach (Seed s in seeds)
        {
            if (InventoryController.Instance.GetAmount(s.itemName) > 0)
            {
                if(!currentSeeds.Contains(s))
                {
                    currentSeeds.Add(s);
                }
            }
            else if (currentSeeds.Contains(s))
            {
                currentSeeds.Remove(s);
            }
        }
        Debug.Log("Updating seeds. Current seeds: " + currentSeeds.Count);
    }

    void Update()
    {
        if(currentSeeds.Count > 0)
        {
            if(!ActionTextController.instance.gameObject.activeSelf)
            {
                ActionTextController.instance.gameObject.SetActive(true);
            }
            CheckTarget();
        }
        else if(ActionTextController.instance.gameObject.activeSelf)
        {
            ActionTextController.instance.gameObject.SetActive(false);
        }
    }

    public void SetAnimator(Animator an)
    {
        anim = an;
    }

    private void CheckTarget()
    {
        GameObject go = RayCastController.instance.GetTarget();
        if (go != null)
        {
            if (go.CompareTag("Hole"))
            {
                ActionTextController.instance.ChangePosition(go.transform.position);
                ActionTextController.instance.ChangeText(currentSeeds[index].itemName);
                if (Input.GetKeyDown(InputManager.instance.Interact) && InventoryController.Instance.GetAmount(currentSeeds[index].itemName) > 0 && InputManager.state != InputManager.States.Working)
                {
                    if (go.transform.childCount < 1)
                    {
                        anim.SetTrigger("Taking");
                        StartCoroutine(AnimDelay(go));
                    }
                }
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
        GameObject loadPlant = DataBase.PlantPrefab(currentSeeds[index].food.itemName);
        GameObject plant = Instantiate(loadPlant, go.transform.position, Quaternion.identity);
        go.GetComponent<MeshRenderer>().material.color = Color.black;
        plant.transform.SetParent(go.transform);
        plant.GetComponent<PlantLife>().SetSeed(currentSeeds[index]);

        InventoryController.Instance.SubstractAmountSeed(1, currentSeeds[index].itemName);
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
