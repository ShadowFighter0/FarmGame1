using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [System.Serializable]
    private class PlayerLevel
    {
        public int level;
        public int experience;
        public PlayerLevel()
        {
            level = 0;
            experience = 0;
        }
        public PlayerLevel(PlayerLevel info)
        {
            level = info.level;
            experience = info.experience;
        }
    }
    private PlayerLevel playerInfo;

    public int[] levelExperience;
    private float barwidth;

    public TextMeshProUGUI currentLvl;
    public TextMeshProUGUI nextLvl;
    public TextMeshProUGUI experience;
    public RectTransform experienceBar;

    public static PlayerManager instace;
    private void Awake()
    {
        instace = this;
        GameEvents.OnSaveInitiated += SavePlayer;
    }
    private void Start()
    {
        if(SaveLoad.SaveExists("PlayerLvl"))
        {
            PlayerLevel loadInfo = SaveLoad.Load<PlayerLevel>("PlayerLvl");
            playerInfo = new PlayerLevel(loadInfo);
        }
        else
        {
            playerInfo = new PlayerLevel();
        }
        barwidth = experienceBar.sizeDelta.x;
        ChangeExpBar();
        ChangeLvls();
    }
    private void SavePlayer()
    {
        SaveLoad.Save(playerInfo, "PlayerLvl");
    }
    public int GetCurrentLevel()
    {
        return playerInfo.level;
    }

    private void ChangeExpBar()
    {
        float size = (float)playerInfo.experience / levelExperience[playerInfo.level] * barwidth;
        experienceBar.sizeDelta = new Vector2(size, experienceBar.sizeDelta.y);
        experience.text = playerInfo.experience + " / " + levelExperience[playerInfo.level];
    }
    private void ChangeLvls()
    {
        currentLvl.text = "Lvl " + playerInfo.level;
        nextLvl.text = "Lvl " + (playerInfo.level + 1);
    }
    public void AddExp(int exp) 
    {
        int nextExp = playerInfo.experience + exp;
        if (nextExp >= levelExperience[playerInfo.level])
        {
            int difference = nextExp - levelExperience[playerInfo.level];
            playerInfo.level++;

            playerInfo.experience = difference;
            
            ChangeLvls();
        }
        else
        {
            playerInfo.experience += exp;
        }
        ChangeExpBar();
        ExperienceBarController.instace.ShowBar();
    }
}
