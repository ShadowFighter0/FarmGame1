using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public Transform male;
    private GameObject glasses;
    void Start()
    {
        glasses = male.GetChild(3).gameObject;
    }

    public void ChangeGlasses()
    {
        glasses.SetActive(!glasses.activeSelf);
    }
    public void ChangeHat(int dir)
    {
        Transform t = male.GetChild(2);
        foreach (Transform child in t)
        {
            if(child.gameObject.activeSelf)
            {
                int index = child.GetSiblingIndex();
                child.gameObject.SetActive(false);
                index += dir;

                if(index < 0)
                {
                    index = t.childCount - 1;
                    t.GetChild(index).gameObject.SetActive(true);
                }
                else if (index > t.childCount - 1)
                {
                    index = 0;
                    t.GetChild(index).gameObject.SetActive(true);
                }
                else
                {
                    t.GetChild(index).gameObject.SetActive(true);
                }
                return;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
