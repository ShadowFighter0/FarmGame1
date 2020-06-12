using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Transform imagesFolder;
    private Image[] images;
    private GameObject[] imageGo;
    private Seed[] seedsToUnlock;
    public GameObject popUp;
    private Shop shopScript;
    public static PlayerManager instace;
    private bool maxLvl = false;
    private void Awake()
    {
        instace = this;
        GameEvents.OnSaveInitiated += SavePlayer;
    }
    private void Start()
    {
        seedsToUnlock = Resources.LoadAll<Seed>("Data/Items/Seeds");
        seedsToUnlock.OrderBy(seeds => seeds.lvl).ToArray();

        if(SaveLoad.SaveExists("PlayerLvl"))
        {
            PlayerLevel loadInfo = SaveLoad.Load<PlayerLevel>("PlayerLvl");
            playerInfo = new PlayerLevel(loadInfo);
        }
        else
        {
            playerInfo = new PlayerLevel();
        }
        int max = imagesFolder.childCount;
        images = new Image[max];
        imageGo = new GameObject[max];
        for (int i = 0; i < max; i++)
        {
            imageGo[i] = imagesFolder.GetChild(i).gameObject;
            images[i] = imageGo[i].GetComponent<Image>();
        }

        barwidth = experienceBar.sizeDelta.x;
        ChangeExpBar();
        ChangeLvls();

        for (int i = 0; i < ShopManager.Instance.shops.Length; i++)
        {
            if (ShopManager.Instance.shops[i].seeds)
            {
                shopScript = ShopManager.Instance.shops[i];
            }
        }
    }

    public Seed[] GetSeedsOrder()
    {
        return seedsToUnlock;
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
        if(!maxLvl)
        {
            int nextExp = playerInfo.experience + exp;
            if (nextExp >= levelExperience[playerInfo.level])
            {
                if(playerInfo.level + 1 >= levelExperience.Length)
                {
                    maxLvl = true;
                    playerInfo.experience = levelExperience[playerInfo.level];
                }
                else
                {
                    int difference = nextExp - levelExperience[playerInfo.level];
                    playerInfo.level++;
                    playerInfo.experience = difference;
                }

                UpdateLvlRewards();
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

    private void UpdateLvlRewards()
    {
        UpdateImages();
        popUp.SetActive(true);
        InputManager.instance.ChangeState(InputManager.States.OnUI);
        ActivateImages();
    }
    private void UpdateImages()
    {
        for (int i = 0; i < imageGo.Length; i++)
        {
            if(imageGo[i].activeSelf)
            {
                imageGo[i].SetActive(false);
            }
        }
    }
    private void ActivateImages()
    {
        int imgIndex = 0;
        for (int i = 0; i < seedsToUnlock.Length; i++)
        {
            if(seedsToUnlock[i].lvl == playerInfo.level)
            {
                imageGo[imgIndex].SetActive(true);
                Sprite spr = DataBase.GetItemSprite(seedsToUnlock[i].itemName);
                images[imgIndex].sprite = spr;
                imgIndex++;

                shopScript.AddToStock(seedsToUnlock[i]);
            }
        }
    }
    public void CloseUI()
    {
        popUp.SetActive(false);
        if(!DialogueSystem.instance.OnDialogue())
        {
            InputManager.instance.ChangeState(InputManager.States.Idle);
        }
    }
}
