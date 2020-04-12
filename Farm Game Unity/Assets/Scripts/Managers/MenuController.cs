using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject exitPopUp;
    void Start()
    {

    }

    public void PauseGame()
    {

    }
    public void NewGame()
    {

    }
    public void Continue()
    {

    }
    public void Options()
    {

    }
    public void ExitPopUp()
    {
        exitPopUp.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}