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
    public Sprite[] sprites;
    private Queue<Sprite> imagesQueue = new Queue<Sprite>();

    public GameObject popUp;

    private Shop shopScript;

    public static PlayerManager instace;
    private void Awake()
    {
        instace = this;
        GameEvents.OnSaveInitiated += SavePlayer;
    }
    private void Start()
    {
        foreach (Sprite s in sprites)
        {
            imagesQueue.Enqueue(s);
        }
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

        StartCoroutine(AddStock());

    }
    private void SavePlayer()
    {
        SaveLoad.Save(playerInfo, "PlayerLvl");
    }
    public int GetCurrentLevel()
    {
        return playerInfo.level;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            AddExp(3);
        }
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
            UpdateLvlRewards();
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

    private void UpdateLvlRewards()
    {
        UpdateImages();
        popUp.SetActive(true);
        InputManager.instance.ChangeState(InputManager.States.OnUI);
        switch (playerInfo.level)
        {
            case 1:
                ActivateImages(2);
                break;
            case 2:
                ActivateImages(4);
                break;
            case 3:
                ActivateImages(4);
                break;
            case 4:
                ActivateImages(5);
                break;
            default:
                break;
        }
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
    private void ActivateImages(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            imageGo[i].SetActive(true);
            Sprite spr = imagesQueue.Dequeue();
            images[i].sprite = spr;
            Item seed = DataBase.GetItem(spr.name);
            shopScript.AddToStock(seed);
        }
    }
    public void CloseUI()
    {
        popUp.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    IEnumerator AddStock()
    {
        yield return new WaitForSeconds(1f);
        Item seed = DataBase.GetItem("CarrotSeed");
        shopScript.AddToStock(seed);
    }
}
