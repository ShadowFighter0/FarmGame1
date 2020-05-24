using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionEntry : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;

    public int position;

    private void Awake()
    {
        nameText = transform.GetChild(0).GetComponent<Text>();
        descriptionText = transform.GetChild(1).GetComponent<Text>();
    }

    public void Fill(QuestInfo quest)
    {
        nameText.text = quest.title;
        descriptionText.text = quest.description;
    }

    public void Button()
    {
        //tonto
    }
}
