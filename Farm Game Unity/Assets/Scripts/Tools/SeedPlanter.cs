using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    public Seed[] seeds;
    private Animator anim;

    private int index = 0;
    private bool onAnim;

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
        GameEvents.OnAnimatorSelected += SetAnimator;
    }
    private void Start()
    {
        
    }

    void Update()
    {
        CheckTarget();
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
                if (Input.GetKeyDown(InputManager.instance.Click) && !onAnim && InventoryController.Instance.GetAmount(seeds[index].itemName) > 0)
                {
                    anim.SetTrigger("Taking");
                    onAnim = true;
                    StartCoroutine(AnimDelay(go));
                }
            }
        }
    }
    IEnumerator AnimDelay(GameObject go)
    {
        yield return new WaitForSeconds(0.3f);
        onAnim = false;
        Plant(go);
    }

    private void Plant(GameObject go)
    {
        if (go.transform.childCount < 1)
        {
            GameObject loadPlant = Resources.Load<GameObject>("Prefabs/" + seeds[index].food.itemName);
            GameObject plant = Instantiate(loadPlant, go.transform.position, Quaternion.identity);
            go.GetComponent<MeshRenderer>().material.color = Color.black;
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
    public void SetSeed(int i)  { Index = i; }
}
