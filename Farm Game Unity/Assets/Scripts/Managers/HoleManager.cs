using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public static HoleManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameEvents.Instance.OnNewDay += NewDay;
    }
    public void NewDay()
    {
        foreach(Transform t in transform)
        {
            t.GetComponent<HoleBehaviour>().NewDay();
        }
    }
}
