using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int day = 1;

    public Image img;
    public float time = 3;
    private Color fadeIn;
    private Color fadeOut;
    private bool loading;
    private bool playerIn;

    public GameObject popUp;

    private void Start()
    {
        fadeIn = new Color32(0, 0, 0, 0);
        fadeOut = new Color32(0, 0, 0, 255);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        if(loading)
        {
            img.color = Color.Lerp(img.color, fadeOut, dt * time);
        }
        else
        {
            img.color = Color.Lerp(img.color, fadeIn, dt * time);
        }

        if (Input.GetKeyDown(InputManager.instance.Interact) && playerIn && InputManager.state != InputManager.States.OnUI)
        {
            OpenPopUp();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            SaveAll();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            DeleteProgress();
        }
    }

    private void OpenPopUp()
    {
        popUp.SetActive(true);
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
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
        GameEvents.Instance.SaveInitiated();
    }
    public void DeleteProgress()
    {
        SaveLoad.SeriouslyDeleteAllSaveFiles();
    }

    private void ChangeDay()
    {
        day++;
        GameEvents.Instance.NewDay();
        InputManager.instance.ChangeState(InputManager.States.Idle);
        SaveAll();
        loading = false;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(time);
        ChangeDay();
    }
    public int GetDay() { return day; }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerIn = false;
    }
}
