using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlantInfo
{
    public int index;
    public string seed;
    public string plant;

    public float x;
    public float y;
    public float z;

    public PlantInfo (int i, string s, string p, Vector3 pos)
    {
        index = i;
        seed = s;
        plant = p;

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
    private bool grownUp = false;

    private float timer = 0;
    private OutlineController outline;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Plants");
        
        if (holeScript != null)
        {
            holeScript = gameObject.GetComponentInParent<HoleController>();
        }
        ChangeModel();

        GameObject lastChild = transform.GetChild(transform.childCount - 1).gameObject;
        outline = lastChild.GetComponentInChildren<OutlineController>();
    }

    public void SetScript(HoleController script) { holeScript = script; }
    private void Update()
    {
        if(holeScript != null)
        {
            if (TimeManager.instance.IsDay() && holeScript.GetWet() && !grownUp)
            {
                float dt = Time.deltaTime;
                timer += dt;
                if (timer > seed.growthTime)
                {
                    timer = 0;
                    UpdateState();
                }
            }

            if(grownUp)
            {
                if(RayCastController.instance.GetTarget() == transform.parent.gameObject)
                {
                    outline.ShowOutline();
                }
                else
                {
                    outline.HideOutline();
                }
            }
        }
    }
    public int GetFood()
    {
        return seed.food.amount; 
    }
    public PlantInfo SavePlant()
    {
        return new PlantInfo(index, seed.itemName, seed.food.itemName, transform.position);
    }
    public void InitializePlant(int i, Seed s)
    {
        index = i;
        SetSeed(s);
    }
    public void SetSeed(Seed s) { seed = s; }

    public bool GetGrownUp() { return grownUp; }

    public void GrownUp()
    {
        grownUp = true;
        index = transform.childCount - 1;
        ChangeModel();
    }

    public void UpdateState()
    {
        bool wet = holeScript.GetWet();
        if (wet && index < transform.childCount - 1)
        {
            index++;
            ChangeModel();
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
            holeScript.SetWater(0);
            StartCoroutine(DeletePlant());
            grownUp = true;
        }
    }
    private IEnumerator DeletePlant()
    {
        yield return new WaitForSeconds(TimeManager.instance.secondsPerDay);
        Destroy(gameObject);
    }

    public void AddInventory()
    { 
        InventoryController.Instance.AddItem(seed.food);
        PlayerManager.instace.AddExp(seed.food.experience);
    }
}
