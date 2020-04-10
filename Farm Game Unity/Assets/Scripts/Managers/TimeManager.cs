using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private int day = 1;

    [SerializeField] Image img = null;
    [SerializeField] float time = 3;
    private Color fadeIn;
    private Color fadeOut;
    private bool loading;
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

    // save stuff here
    private void ChangeDay()
    {
        day++;
        //CAMBIAR ESTO Y UTILIZAR UN EVENTO NEW DAY
        //all mamagers here
        HoleManager.instance.NewDay();
        InputManager.instance.ChangeState(InputManager.States.Idle);
        loading = false;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(time);
        ChangeDay();
    }
    public int GetDay() { return day; }
}
