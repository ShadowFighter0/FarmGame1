using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class QuestPanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private QuestInfo quest;
    [SerializeField] private GameObject infoPanel = null;
    private Text text;

    private void Start()
    {
        text = infoPanel.transform.GetChild(0).GetComponent<Text>();
    }

    public void AddQuest()
    {
        if(quest != null)
        {
            QuestController.Instance.AddQuest(new SampleQuest(quest.title, quest.npcName, quest.description, quest.ids, quest.amounts, quest.itemReward));
        }
    }
    public string QuestInfo()
    {
        string info = quest.title + "\n" + quest.description + " " + quest.npcName + "\n";
        for (int i = 0; i < quest.ids.Length; i++)
        {
            info += "- " + quest.amounts[i] + " " + quest.ids[i] + "\n";
        }
        return info;
    }

    public void AssignQuest(QuestInfo q) { quest = q; }

    public void OnPointerClick(PointerEventData eventData)
    {
        AddQuest();
        gameObject.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetActive(true);
        text.text = QuestInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }
}
