using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    private MeshRenderer rend;
    public Color wetColor;
    public Color dryColor;

    private bool wet = false;
    private float water = 0;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.color = dryColor;
        GameEvents.OnNewDay += NewDay;
    }

    private void NewDay()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<PlantLife>().UpdateState();
        }
        if (!wet)
        {
            transform.SetParent(FindObjectOfType<ObjectPooler>().transform);
            gameObject.SetActive(false);
        }
        water = 0;
        wet = false;
    }

    public void AddWater(float amount)
    {
        water += amount;
        CheckWater();
    }

    public float GetWater() { return water; }
    public void SetWater(float w) 
    { 
        water = w;
        CheckWater();
    }

    private void CheckWater()
    {
        if (water >= 100)
        {
            //play sound
            wet = true;
            rend.material.color = wetColor;
        }
        else
        {
            wet = false;
            rend.material.color = dryColor;
        }
    }

    public bool GetWet()
    {
        return wet;
    }
    private void OnDestroy()
    {
        GameEvents.OnNewDay -= NewDay;
    }
}
