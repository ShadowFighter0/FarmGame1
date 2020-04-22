using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHat : MonoBehaviour
{
    public GameObject glasses;
    public Transform hatFolder;
    private Animator anim;
    private int index = 0;
    void Start()
    {
        anim = GetComponent<Animator>();
        foreach (Transform child in hatFolder)
        {
            if (child.gameObject.activeSelf)
            {
                index = child.GetSiblingIndex();
                return;
            }
        }
    }

    public void Finish()
    {
        anim.SetTrigger("Finished");
    }
    public void Selected()
    {
        anim.SetTrigger("Selected");
    }
    public void ChangeGlasses()
    {
        anim.SetTrigger("Rand");
        glasses.SetActive(!glasses.activeSelf);
    }
    public void NewHat(int dir)
    {
        anim.SetTrigger("Rand");
        hatFolder.GetChild(index).gameObject.SetActive(false);
        index += dir;

        if (index < 0)
        {
            index = hatFolder.childCount - 1;
        }
        else if (index > hatFolder.childCount - 1)
        {
            index = 0;
        }

        hatFolder.GetChild(index).gameObject.SetActive(true);
    }
}
