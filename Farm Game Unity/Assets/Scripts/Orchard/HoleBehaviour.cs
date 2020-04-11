using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour, INewDay
{
    private MeshRenderer rend;
    private Color wetColor;
    private Color dryColor;

    private bool wet = false;
    private float water = 0;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        wetColor = new Color32(165, 60, 38, 255);
        dryColor = new Color32(231, 178, 96, 255);
        rend.material.color = dryColor;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (water >= 100)
        {
            //play sound
            wet = true;
        }
        else
        {
            wet = false;
        }
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
        if(transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<PlantLife>().NewDay();
        }

        if (!wet)
        {
            gameObject.SetActive(false);
        }
        water = 0;
    }

    public void AddWater(float amount) //hacer que se pueda desbordar y ahogar
    {
        water += amount;
    }

    public bool GetWet()
    {
        return wet;
    }
}
