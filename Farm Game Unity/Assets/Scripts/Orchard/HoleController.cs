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

    void Update()
    {
        float dt = Time.deltaTime;
        UpdateColor(dt);
    }

    private void UpdateColor(float dt)
    {
        if (wet)
        {
            rend.material.color = Color.Lerp(rend.material.color, wetColor, dt);
        }
        else
        {
            rend.material.color = Color.Lerp(rend.material.color, dryColor, dt);
        }
    }

    public void NewDay()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<PlantLife>().NewDay();
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
        }
        else
        {
            wet = false;
        }
    }

    public bool GetWet()
    {
        return wet;
    }
}
