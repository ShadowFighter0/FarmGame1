﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    private MeshRenderer rend;
    public Color wetColor;
    public Color dryColor;

    private bool wet = false;
    private float water = 0;

    public float Timer { get; set; }
    public float time;

    private bool soundPlayed = false;

    private AudioClip wetSound;
    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.color = dryColor;
    }
    private void Start()
    {
        wetSound = DataBase.SearchClip("Wet");
        time = GameManager.instance.dayTime;
    }
    private void Update()
    {
        float dt = Time.deltaTime;
        Timer += dt;

        if(Timer > time)
        {
            Timer = 0;
            UpdateHole();
        }
    }

    private void UpdateHole()
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
        Timer = 0;
        CheckWater();
    }

    private void CheckWater()
    {
        if (water >= 100)
        {
            if(!soundPlayed)
            {
                soundPlayed = true;
                AudioManager.PlaySound(wetSound);
            }
            wet = true;
            rend.material.color = wetColor;
        }
        else
        {
            soundPlayed = false;
            wet = false;
            rend.material.color = dryColor;
        }
    }

    public bool GetWet()
    {
        return wet;
    }
}
