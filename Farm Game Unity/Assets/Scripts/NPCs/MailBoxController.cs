 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxController : MonoBehaviour
{
    private bool playerNear;

    private Queue<QuestInfo> quests = new Queue<QuestInfo>();
    private Queue<Mail> mails = new Queue<Mail>();
    public Transform mailFolder;
    private GameObject mailsPanel;
    public int offset = 70;

    public Mail[] mail;

    private List<GameObject> panels;

    private bool done = true;

    private int activePanels = 0;
    private int index = 0;

    private void Start()
    {
        mailsPanel = mailFolder.parent.gameObject;
        GameEvents.Instance.OnNewDay += NewDay;
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(mail[0]);
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(mail[1]);
        AddContent(QuestFileInfo.Instance.GetQuest());
        FillMails();
    }
    private void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(InputManager.instance.Interact) && done)
            {
                mailsPanel.SetActive(true);

                panels = new List<GameObject>();
                for (int i = 0; i < activePanels; i++)
                {
                    panels.Add(mailFolder.GetChild(i).gameObject);
                    panels[i].SetActive(true);
                }
                index = activePanels - 1;
                InputManager.instance.ChangeState(InputManager.States.OnUI);
                done = false;
            }
            if ((Input.GetKeyDown(KeyCode.F1) || GetActiveMails() == 0) && !done)
            {
                Close();
            }
        }
    }

    public void Close()
    {
        mailsPanel.SetActive(false);
        done = true;
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    private void NewDay()
    {
        
    }

    public void ChangePage(int dir)
    {
        int oldIndex = index;
        index += dir;
        if(index < 0)
        {
            index = 0;
        }
        if (index > activePanels - 1)
        {
            index = activePanels - 1;
        }
        if (index != oldIndex)
        {
            Vector3 pos = Vector3.right * dir * offset;
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].GetComponent<RectTransform>().localPosition += pos;
            }

            panels[index].transform.SetSiblingIndex(activePanels - 1);
        }
    }

    private int GetActiveMails()
    {
        int n = 0;
        foreach (Transform child in mailFolder)
        {
            if(child.gameObject.activeSelf)
            {
                n++;
            }
            else
            {
                break;
            }
        }
        return n;
    }
    

    public void AddContent(QuestInfo q) 
    {
        activePanels++;
        quests.Enqueue(q); 
    }
    public void AddContent(Mail m) 
    {
        activePanels++;
        mails.Enqueue(m); 
    }

    public void TakeMail()
    {
        panels[index].GetComponent<MailBoxPanel>().AddContent();
        panels.RemoveAt(index);
        activePanels--;
        ChangePage(index--);
    }

    private void FillMails()
    {
        Vector2 pos = Vector2.right * activePanels * offset;
        int index = 0;
        int n = quests.Count + mails.Count;
        foreach (Transform child in mailFolder)
        {
            if (index >= n)
            {
                return;
            }
            
            if (quests.Count > 0)
            {
                child.gameObject.GetComponent<MailBoxPanel>().Assign(quests.Dequeue());
            }
            else if (mails.Count > 0)
            {
                child.gameObject.GetComponent<MailBoxPanel>().Assign(mails.Dequeue());
            }

            child.gameObject.GetComponent<RectTransform>().localPosition = pos;

            pos -= Vector2.right * offset;
            index++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
