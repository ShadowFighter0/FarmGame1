using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private float _targetDayLength = 0.5f;
    public float targetDayLength
    {
        get
        {
            return _targetDayLength;
        }
    }

    [SerializeField]
    [Range(0f, 1f)]
    private float _timeOfDay;
    public float timeOfDay
    {
        get
        {
            return _timeOfDay;
        }
    }

    private float _timeScale = 100f;

    [SerializeField]
    private int _yearLength = 100;
    public float yearLength
    {
        get
        {
            return _yearLength;
        }
    }

    public bool pause = false;

    public Transform dailyRotation;

    public Light sun;
    private float sunIntensity;
    public float sunBaseIntensity = 1.1f;

    [SerializeField]
    private AnimationCurve timeCurve;
    private float timeCurveNormalization;

    private List<DNModuleBase> moduleList = new List<DNModuleBase>();

    private void Start()
    {
        NormalTimeCurve();
        _targetDayLength = TimeManager.instance.secondsPerDay / 60;
    }

    private void Update()
    {
        UpdateTime();

        AdjustSunRotation();
        SunIntensity();
        UpdateModule(); //will update modules each frame
    }

    public void UpdateTime()
    {
        _timeOfDay = ((TimeManager.instance.time.minute * 60) + TimeManager.instance.timer) / 86400; // seconds in a day

        if (_timeOfDay > 1) //new day!!
            {
                _timeOfDay -= 1;
            }
    }

    public void AddModule (DNModuleBase module)
    {
        moduleList.Add(module);
    }

    public void RemoveModule(DNModuleBase module)
    {
        moduleList.Remove(module);
    }

    public void UpdateModule()
    {
        foreach (DNModuleBase module in moduleList)
        {
            module.UpdateModule(sunIntensity);
        }
    }

    public void SunIntensity()
    {
        sunIntensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        sunIntensity = Mathf.Clamp01(sunIntensity);

        sun.intensity = sunIntensity * sunBaseIntensity ;
    }

    public void AdjustSunRotation()
    {
        float sunAngle = timeOfDay * 360f;
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 0f, 0f));
    }
    private void UpdateTimeScale()
    {
        _timeScale = 24 / (_targetDayLength / 60);
        _timeScale *= timeCurve.Evaluate(timeOfDay);
        _timeScale /= timeCurveNormalization;
    }

    private void NormalTimeCurve()
    {
        float stepSize = 0.01f;
        int numberSteps = Mathf.FloorToInt(1f / stepSize);
        float curveTotal = 0;

        for (int i = 0; i < numberSteps; i++)
        {
            curveTotal += timeCurve.Evaluate(i * stepSize);
        }

        timeCurveNormalization = curveTotal / numberSteps;
    }
}