﻿using System.Collections;
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
    public void Selected()
    {
        //anim.SetBool("Selected", true);
    }
    public void ChangeGlasses()
    {
        glasses.SetActive(!glasses.activeSelf);
    }
    public void NewHat(int dir)
    {
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
