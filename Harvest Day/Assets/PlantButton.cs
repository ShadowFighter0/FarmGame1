using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantButton : MonoBehaviour
{
    public bool plant;

    public static PlantButton Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Click()
    {
        if (plant)
        {
            SeedPlanter.instance.actionByButton = true;
            plant = false;
        }
    }
}
