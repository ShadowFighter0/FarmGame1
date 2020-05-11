using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Date
{
    public int day;
    public int hour;
    public int minute;
    public Date()
    {
        day = 1;
        hour = 7;
        minute = 0;
    }
    public Date(Date t)
    {
        day = t.day;
        hour = t.hour;
        minute = t.minute;
    }
}
public class TimeManager : MonoBehaviour
{
    private Date time;
    private float timer;
    public int minuteAmount;
    private float minuteTime;

    private bool resting = false;
    private int wakeHour = 0;
    private int wakeMinute = 0;

    private int textHour = 1;

    public int secondsPerDay = 300;
    
    private bool playerIn = false;

    private const float minutesPerDay = 1440.0f;

    public Slider hoursSlider;
    public Text hoursText;
    public Text timeText;
    public Text wakeUpText;

    public GameObject exitPopUp;

    public static TimeManager instance;
    private void Awake()
    {
        instance = this;
        GameEvents.OnSaveInitiated += SaveTime;
    }
    private void Start()
    {
        minuteTime = secondsPerDay * minuteAmount / minutesPerDay;

        if (SaveLoad.SaveExists("GameTime"))
        {
            time = new Date(SaveLoad.Load<Date>("GameTime"));
        }
        else
        {
            time = new Date();
        }
        timeText.text = time.hour + ":" + time.minute;
        SetWakeHourText();
        hoursSlider.onValueChanged.AddListener(delegate { SetHoursText(); });
    }

    private void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            timer += Time.deltaTime;
            if (timer > minuteTime)
            {
                timer = 0;
                time.minute += minuteAmount;

                if (time.minute == 60)
                {
                    time.minute = 0;
                    time.hour++;

                    if(time.hour == 24)
                    {
                        time.hour = 0;
                    }
                    if (time.hour == 7)
                    {
                        time.day++;
                        GameEvents.NewDay();
                    }
                }
                if (resting)
                {
                    if (time.hour == wakeHour && time.minute == wakeMinute)
                    {
                        resting = false;
                        Time.timeScale = 1;
                        InputManager.instance.ChangeState(InputManager.States.Idle);
                        //AudioManager.PlaySound(DataBase.SearchClip("Alarm"));
                    }
                }
                timeText.text = time.hour + ":" + time.minute;
                if(exitPopUp.activeSelf)
                {
                    SetWakeHourText();
                }
            }
            if (Input.GetKeyDown(InputManager.instance.Interact) && playerIn && InputManager.state == InputManager.States.Idle)
            {
                OpenPopUp();
            }
        }
    }

    public void Rest()
    {
        if(!resting)
        {
            exitPopUp.SetActive(false);
            Time.timeScale = 10;
            int targetHour = time.hour + (int)hoursSlider.value;
            if (targetHour >= 24)
            {
                targetHour -= 24;
                wakeHour = targetHour;
                wakeMinute = time.minute;
            }
            else
            {
                wakeHour = targetHour;
                wakeMinute = time.minute;
            }

            resting = true;
            InputManager.instance.ChangeState(InputManager.States.Sleeping);
        }
    }
    private void SaveTime()
    {
        SaveLoad.Save(time, "GameTime");
    }
    private void OpenPopUp()
    {
        exitPopUp.SetActive(true);
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }
    public void ClosePopUp()
    {
        exitPopUp.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
    private void SetHoursText()
    {
        textHour = (int)hoursSlider.value;

        if (textHour == 1)
        {
            hoursText.text = textHour + " hour";
        }
        else
        {
            hoursText.text = textHour + " hours";
        }
        SetWakeHourText();
    }
    private void SetWakeHourText()
    {
        int wakeTime = time.hour + textHour;
        if(wakeTime >= 24)
        {
            wakeTime -= 24;
        }
        wakeUpText.text = "Wake up hour: " + wakeTime + ":" + time.minute;
    }

    public bool ActiveHour() 
    {
        int hour = time.hour;
        return hour <= 22 && hour >= 7; 
    }
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
