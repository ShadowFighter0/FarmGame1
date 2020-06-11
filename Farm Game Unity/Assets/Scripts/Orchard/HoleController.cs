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

    public float Timer { get; set; }
    public float time;

    private bool soundPlayed = true;
    private AudioClip wetSound;
    public GameObject vfx;
    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.color = dryColor;
    }
    private void Start()
    {
        wetSound = DataBase.GetAudioClip("Wet");
        time = TimeManager.instance.secondsPerDay;
        soundPlayed = false;
        time = TimeManager.instance.secondsPerDay;
    }
    private void Update() {
        Timer += Time.deltaTime;
        if(Timer > time)
        {
            UpdateHole();
        }
    }
    private void UpdateHole()
    {
        if (!wet && transform.childCount == 0)
        {
            transform.SetParent(FindObjectOfType<ObjectPooler>().transform);
            gameObject.SetActive(false);
        }
        SetWater(0);
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
    public bool HasPlant()
    {
        return transform.childCount > 0;
    }
    public bool GetWet()
    {
        return wet;
    }
}
