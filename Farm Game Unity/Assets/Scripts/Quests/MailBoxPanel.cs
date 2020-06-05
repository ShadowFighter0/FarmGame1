using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxPanel : MonoBehaviour
{
    private QuestInfo quest;
    private Mail mail;

    public void AddContent()
    {
        if (quest != null)
        {
            QuestController.Instance.AddQuest(new SampleQuest(quest));
        }
        if (mail != null)
        {
            if (mail.rewardItem != null)
            {
                for (int i = 0; i < mail.rewardItem.Length; i++)
                {
                    InventoryController.Instance.AddItem(mail.rewardItem[i], mail.amount[i]);
                }
            }
        }
    }
    public string QuestInfo()
    {
        string info = quest.questName + "\n" + quest.description;
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
            quest = new QuestInfo(q);
        }
        transform.GetChild(0).GetComponent<Text>().text = m.message;
        transform.SetSiblingIndex(0);
    }
}
