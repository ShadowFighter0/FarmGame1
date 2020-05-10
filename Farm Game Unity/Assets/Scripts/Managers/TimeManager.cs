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
    public float minuteTime;
    public int minuteAmount;

    public int maxHour;
    public int minHour;

    private bool resting = false;
    private int wakeHour = 0;
    private int wakeMinute = 0;

    public int dayTime;

    public Text timeText;

    public static TimeManager instance;
    private void Awake()
    {
        instance = this;
        GameEvents.OnSaveInitiated += SaveTime;
    }
    private void Start()
    {
        int dayHours = (maxHour + 24) - minHour;
        int min = 60 / minuteAmount;
        int minutesPerDay = min * dayHours;
        minuteTime = dayTime / minutesPerDay;

        if (SaveLoad.SaveExists("GameTime"))
        {
            time = new Date(SaveLoad.Load<Date>("GameTime"));
        }
        else
        {
            time = new Date();
        }
        timeText.text = time.hour + ":" + time.minute;
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
                    if (time.hour == maxHour)
                    {
                        time.hour = minHour;
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
            }
        }
    }

    public void Rest(int hours)
    {
        if(!resting)
        {
            Time.timeScale = hours * 5;
            int targetHour = time.hour + hours;
            if (targetHour >= 24)
            {
                targetHour -= 24;
                if (targetHour >= maxHour)
                {
                    int dif = targetHour - maxHour;
                    wakeHour = minHour + dif;
                    wakeMinute = time.minute;
                }
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
}
