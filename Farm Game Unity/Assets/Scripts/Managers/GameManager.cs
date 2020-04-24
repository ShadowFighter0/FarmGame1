using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

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
    public float time = 3;
    private Color fadeIn = new Color32(0, 0, 0, 0);
    private Color fadeOut = new Color32(0, 0, 0, 255);

    private bool loading;
    private bool playerIn;
    private bool gamePaused = false;

    private DateTime lastTimeSaved;

    public GameObject UIFolder;
    public GameObject exitPopUp;
    public GameObject saveText;
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject continueText;
    public GameObject options;
    public Text lastSavedText;
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

    private bool newGame;

    private string[] sentences = {"CONTINUE", "NEW GAME"};
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
    }
    private void Start()
    {
        if (SaveLoad.HasSaves())
        {
            newGame = false;
            continueText.transform.GetChild(0).GetComponent<Text>().text = sentences[0];
            PlayerInfo info = SaveLoad.Load<PlayerInfo>("PlayerInfo");
            if(info.modelIndex == 1)
            {
                player.GetChild(0).gameObject.SetActive(false);
                maleHats.GetChild(info.hatIndex).gameObject.SetActive(true);
                maleGlasses.SetActive(info.hasGlasses);

                GameEvents.AnimatorSelected(femaleAnim);
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

                GameEvents.AnimatorSelected(femaleAnim);
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
            newGame = true;
            continueText.transform.GetChild(0).GetComponent<Text>().text = sentences[1];
            InputManager.instance.ChangeState(InputManager.States.OnUI);
        }

        oriPos = player.position + Vector3.up;
        oriRot = player.rotation;
    }
    private void SendAnimators(Animator a)
    {
        GameEvents.AnimatorSelected(a);
    }
    private void Update()
    {
        float dt = Time.deltaTime;
        if(loading)
        {
            fade.color = Color.Lerp(fade.color, fadeOut, dt * time);
        }
        else
        {
            fade.color = Color.Lerp(fade.color, fadeIn, dt * time);
        }

        if (Input.GetKeyDown(InputManager.instance.Interact) && playerIn && InputManager.state != InputManager.States.OnUI)
        {
            OpenPopUp();
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            SaveAll();
        }
        if(Input.GetKeyDown(KeyCode.F8))
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DeleteProgress();
        }

        if(Input.GetKeyDown(KeyCode.P) && !mainMenu.activeSelf)
        {
            PauseGame();
        }

        if (pauseMenu.activeSelf)
        {
            int hour = lastTimeSaved.Hour;
            int min = lastTimeSaved.Minute;
            int sec = lastTimeSaved.Second;
            if (hour != 0 && min != 0 && sec != 0)
            {
                UpdateSaveText(DateTime.Now);
            }
        }
    }

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
        bool paused = pauseMenu.activeSelf;
        paused = !paused;
        gamePaused = paused;
        pauseMenu.SetActive(gamePaused);
        if (gamePaused)
        {
            //Time.timeScale = 0;
            InputManager.instance.ChangeState(InputManager.States.OnUI);
        }
        else
        {
            //Time.timeScale = 1;
            InputManager.instance.ChangeState(InputManager.States.Idle);
        }
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

    public void ContinueGame()
    {
        if(newGame)
        {
            hudCustom.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            PlayerFollow.instance.enabled = true;
            InputManager.instance.enabled = true;
            InputManager.instance.ChangeState(InputManager.States.Idle);
            mainMenu.SetActive(false);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void UpdateSaveText(DateTime time)
    {
        int difference = time.Minute - lastTimeSaved.Minute;
        string t = lastTimeSaved.Hour + ":" + lastTimeSaved.Minute + ":" + lastTimeSaved.Second;
        lastSavedText.text = "(Last save at " + t + ". " + difference + " minutes ago)";
    }
    #endregion
    private void OpenPopUp()
    {
        exitPopUp.SetActive(true);
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }

    public void NewDay()
    {
        if(!loading)
        {
            InputManager.instance.ChangeState(InputManager.States.Sleeping);
            loading = true;
            StartCoroutine(Fade());
        }
    }

    public void SaveAll()
    {
        GameEvents.SaveInitiated();
        saveText.SetActive(true);
        DateTime time = DateTime.Now;
        lastTimeSaved = time;
        if (pauseMenu.activeSelf)
        {
            UpdateSaveText(time);
        }
        
        Debug.Log("Last save: " + time.ToString());
        StartCoroutine(DisableSaveText());
    }

    public void DeleteProgress()
    {
        SaveLoad.DeleteAllData();
        Reload();
    }

    private void ChangeDay()
    {
        day++;
        GameEvents.NewDay();
        InputManager.instance.ChangeState(InputManager.States.Idle);
        loading = false;
        SaveAll();
    }
    IEnumerator DisableSaveText()
    {
        yield return new WaitForSeconds(3);
        saveText.SetActive(false);
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(time);
        ChangeDay();
    }
    public int GetDay() { return day; }

    #region Trigger with player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = false;
        }
    }
    #endregion
}
