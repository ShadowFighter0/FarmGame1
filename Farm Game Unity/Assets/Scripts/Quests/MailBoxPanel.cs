using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MailBoxPanel : MonoBehaviour
{
    private QuestInfo quest;
    private Mail mail;

    public void AddContent()
    {
        if(quest != null)
        {
            QuestController.Instance.AddQuest(new SampleQuest(quest.title, quest.npcName, quest.description, quest.ids, quest.amounts, quest.itemReward));
        }
        if(mail != null)
        {
            InventoryController.Instance.AddItem(mail.rewardItem);
        }
        gameObject.SetActive(false);
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

    public void Assign(QuestInfo q) 
    { 
        quest = q;
        transform.GetChild(0).GetComponent<Text>().text = QuestInfo();
    }
    public void Assign(Mail m)
    {
        mail = m;
        transform.GetChild(0).GetComponent<Text>().text = m.message;
    }
}
