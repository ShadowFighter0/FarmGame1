using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;

[System.Serializable]
public class PlayerInfo
{
    public int day;

    public int hatIndex;
    public int modelIndex;
    public bool hasGlasses;
    
    public float x;
    public float y;
    public float z;

    public PlayerInfo(Vector3 pos, int d, int hat, int model, bool glasses)
    {
        day = d;

        x = pos.x;
        y = pos.y;
        z = pos.z;

        hatIndex = hat;
        modelIndex = model;
        hasGlasses = glasses;
    }
}

public class GameManager : MonoBehaviour
{
    private int day = 1;

    public Image fade;
    public float time = 1;

    private Color fadeIn = new Color32(32, 32, 32, 0);
    private Color fadeOut = new Color32(32, 32, 32, 255);
    private bool gamePaused = false;

    private DateTime lastTimeSaved;

    public GameObject UIFolder;

    public GameObject saveText;
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject continueText;
    public GameObject options;
    public GameObject menuOptions;
    public static GameManager instance;

    public Transform player;
    private Vector3 oriPos;
    private Quaternion oriRot;

    private int hatIndex;
    private int modelIndex;
    private bool hasGlasses;

    public Transform maleHats;
    public Transform femaleHats;

    public GameObject maleGlasses;
    public GameObject femaleGlasses;

    public GameObject hudCustom;

    public Transform maleTools;
    public Transform femaleTools;

    public Animator maleAnim;
    public Animator femaleAnim;

    public RuntimeAnimatorController controller;

    private bool newGame = false;
    private bool gameLoaded = false;
    public bool gameStarted = false;

    public Transform cam;
    private Quaternion newCamRot;
    private Vector3 newCamPos;
    private bool camMovementFinished;

    public Transform startCameraPivot;
    public Transform mainMenuCameraPivot;

    private float currentLerpTime;
    private float lerpTime = 1;

    private string[] sentences = {"CONTINUE", "NEW GAME"};

    private bool canSave = true;

    private AudioClip mainMenuMusic;

    public float MouseSensivility { get; set; }
    private void Awake()
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        if (instance != null && instance != this)
        {
            Debug.Log("deleting double singleton");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        GameEvents.OnNewDay += SaveAll;
    }
    private void Start()
    {
        MouseSensivility = 300;
        mainMenuMusic = DataBase.GetAudioClip("Theme");
        if (SaveLoad.HasSaves())
        {
            newGame = false;
            continueText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sentences[0];
            PlayerInfo info = SaveLoad.Load<PlayerInfo>("PlayerInfo");
            if(info.modelIndex == 1)
            {
                player.GetChild(0).gameObject.SetActive(false);
                maleHats.GetChild(info.hatIndex).gameObject.SetActive(true);
                maleGlasses.SetActive(info.hasGlasses);

                InputManager.instance.SetAnimator(maleAnim);
                InputManager.instance.SetTools(maleTools);

                maleAnim.runtimeAnimatorController = controller;

                maleAnim.gameObject.transform.SetParent(null);

                player.position = maleAnim.gameObject.transform.position;
                player.rotation = maleAnim.gameObject.transform.rotation;

                maleAnim.gameObject.transform.SetParent(player);
            }
            else
            {
                player.GetChild(1).gameObject.SetActive(false);
                femaleHats.GetChild(info.hatIndex).gameObject.SetActive(true);
                femaleGlasses.SetActive(info.hasGlasses);

                InputManager.instance.SetAnimator(femaleAnim);
                InputManager.instance.SetTools(femaleTools);

                femaleAnim.runtimeAnimatorController = controller;

                femaleAnim.gameObject.transform.SetParent(null);

                player.position = femaleAnim.gameObject.transform.position;
                player.rotation = femaleAnim.gameObject.transform.rotation;

                femaleAnim.gameObject.transform.SetParent(player);
            }
            day = info.day;
            player.position = new Vector3(info.x, info.y, info.z);

            Destroy(FindObjectOfType<CharacterSelect>());

            hudCustom.SetActive(false);
        }
        else
        {
            gameStarted = false;
            newGame = true;
            continueText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sentences[1];
            InputManager.instance.ChangeState(InputManager.States.OnUI);
        }

        oriPos = player.position + Vector3.up;
        oriRot = player.rotation;

        fade.color = fadeOut;

        cam.position = startCameraPivot.position;
        cam.rotation = startCameraPivot.rotation;

        newCamPos = cam.position;
        newCamRot = cam.rotation;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitStart());
        SceneManager.sceneLoaded -= SceneLoaded;
    }
    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(0f);
        gameLoaded = true;
        //AudioManager.PlaySound(mainMenuMusic);
        currentLerpTime = 0;

        yield return new WaitForSeconds(0f);
        newCamPos = mainMenuCameraPivot.position;
        currentLerpTime = 0;

        yield return new WaitForSeconds(0f);
        mainMenu.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if(gameLoaded)
        {
            float dt = Time.deltaTime;

            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            fade.color = Color.Lerp(fade.color, fadeIn, dt * time);

            if(!camMovementFinished)
            {
                float t = currentLerpTime / lerpTime;
                t = t * t * t * (t * (6f * t - 15f) + 10f);

                cam.position = Vector3.Lerp(cam.position, newCamPos, t);
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveAll();
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                Reload();
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                DeleteProgress();
            }

            if (Input.GetKeyDown(KeyCode.P) && !mainMenu.activeSelf)
            {
                PauseGame();
            }
        }
    }

    public void SetCamPos(Vector3 pos)
    {
        newCamPos = pos;
        currentLerpTime = 0;
    }
    public void FreeCam() { camMovementFinished = true; }
    public void SavePlayer()
    {
        PlayerInfo info = new PlayerInfo(player.position, day, hatIndex, modelIndex, hasGlasses);
        SaveLoad.Save(info, "PlayerInfo");
    }

    public void SetPlayerCustomization(int h, int m, bool g)
    {
        hatIndex = h;
        modelIndex = m;
        hasGlasses = g;
    }
    #region Pause menu
    public void PauseGame()
    {
        gamePaused = !pauseMenu.activeSelf;
        pauseMenu.SetActive(gamePaused);
        if (gamePaused)
        {
            Time.timeScale = 0;
            InputManager.instance.ChangeState(InputManager.States.OnUI);
        }
        else
        {
            Time.timeScale = 1;
            InputManager.instance.ChangeState(InputManager.States.Idle);
        }
    }
    
    public void SetSensivility(Slider value)
    {
        MouseSensivility = value.value;
    }
    public void Respawn()
    {
        player.position = oriPos;
        player.rotation = oriRot;
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    public void ActiveOptions()
    {
        options.SetActive(!options.activeSelf);
    }
    public void ActiveMenuOptions()
    {
        menuOptions.SetActive(!menuOptions.activeSelf);
    }


    public void ContinueGame()
    {
        if(newGame)
        {
            hudCustom.SetActive(true);
            mainMenu.SetActive(false);
            CharacterSelect.instance.enabled = true;
        }
        else
        {
            cam.position = player.position;
            FreeCam();

            PlayerFollow.instance.enabled = true;
            InputManager.instance.enabled = true;
            InputManager.instance.ChangeState(InputManager.States.Idle);
            mainMenu.SetActive(false);
            gameStarted = true;
        }
    }
    public void StartNewGame()
    {
        if(SaveLoad.HasSaves())
        {
            DeleteProgress();
            Reload();
        }
        
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Reload()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    public void SaveAll()
    {
        if(canSave)
        {
            canSave = false;

            GameEvents.SaveInitiated();
            saveText.SetActive(true);
            DateTime time = DateTime.Now;
            lastTimeSaved = time;

            Debug.Log("Last save: " + time.ToString());
            StartCoroutine(DisableSaveText());
        }
    }

    public void DeleteProgress()
    {
        SaveLoad.DeleteAllData();
        Reload();
    }
    IEnumerator DisableSaveText()
    {
        yield return new WaitForSeconds(3);
        saveText.SetActive(false);
        yield return new WaitForSeconds(1);
        canSave = true;
    }
}
