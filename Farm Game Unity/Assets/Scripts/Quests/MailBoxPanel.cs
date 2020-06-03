﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxPanel : MonoBehaviour
{
    private QuestInfo quest;
    private Mail mail;

    public void AddContent()
    {
        gameObject.SetActive(false);
        if (quest != null)
        {
            QuestController.Instance.AddQuest(new SampleQuest(quest.title, quest.npcName, quest.description, quest.ids, quest.amounts, quest.itemReward, quest.experience, quest.isOrder));
        }
        if (mail != null)
        {
            if (mail.rewardItem != null)
            {
                //InventoryController.Instance.AddItem(mail.rewardItem);
            }
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
    
    public void Assign(QuestInfo q) 
    { 
        quest = q;
        transform.GetChild(0).GetComponent<Text>().text = QuestInfo();
        transform.SetSiblingIndex(0);
    }
    public void Assign(Mail m)
    {
        mail = m;

        QuestTemplate q = m.quest;
        if (q != null)
        {
            quest = new QuestInfo(q.title, q.description, q.ids, q.amounts, q.NPCName, q.itemReward, q.experience, q.isOrder);
        }
        transform.GetChild(0).GetComponent<Text>().text = m.message;
        transform.SetSiblingIndex(0);
    }
}
