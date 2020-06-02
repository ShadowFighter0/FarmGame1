using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggersController : MonoBehaviour
{
    public static TutorialTriggersController instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        bool[] bools = SaveLoad.Load<bool[]>("TutorialTriggers");
        for (int i = 0; i < bools.Length; i++)
        {
            transform.GetChild(i).gameObject.SetActive(bools[i]);
        }
    }

    public void SaveTriggers()
    {
        int mx = transform.childCount;
        bool[] b = new bool[mx];
        for (int i = 0; i < mx; i++)
        {
            b[i] = transform.GetChild(i).gameObject.activeSelf;
        }
        SaveLoad.Save(b, "TutorialTriggers");
    }
}
