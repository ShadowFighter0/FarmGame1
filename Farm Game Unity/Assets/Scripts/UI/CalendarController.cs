using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    private Transform dayFolder;
    private Text month;
    private void Start()
    {
        dayFolder = transform.GetChild(0);

        int day = 1;
        foreach (Transform child in dayFolder)
        {
            child.GetChild(0).GetComponent<Text>().text = "DAY " + day;
            switch (day)
            {
                case 2:
                    child.GetChild(1).GetComponent<Text>().text = "Paga la coca primer aviso";
                    break;
                case 10:
                    child.GetChild(1).GetComponent<Text>().text = "Paga la coca primer aviso";
                    break;
                default:
                    child.GetChild(1).GetComponent<Text>().text = "";
                    break;
            }
            
            day++;
        }
        month = transform.GetChild(1).GetComponent<Text>();
        month.text = "FIRST MONTH";

        SetDayText("El alquiler crack", 6);
        SetDayText("no se tio ", 2);
        SetDayText("te llegan los limones", 6);
    }

    public void SetDayText(string text, int day)
    {
        dayFolder.GetChild(day - 1).GetChild(1).GetComponent<Text>().text += text + "\n";
    }
}
