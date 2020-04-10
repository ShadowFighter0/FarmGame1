using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxController : MonoBehaviour
{
    private bool playerNear;

    private Queue<QuestInfo> quests = new Queue<QuestInfo>();
    private Queue<Mail> mails = new Queue<Mail>();
    public Transform questFolder;
    public int offset = 70;

    public Mail[] mail;

    private bool done = true;

    private int activePanels = 0;

    private void Start()
    {
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(mail[0]);
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(mail[1]);
        AddContent(QuestFileInfo.Instance.GetQuest());
    }
    private void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.Interact) && playerNear && done)
        {
            ShowMails();
            InputManager.instance.ChangeState(InputManager.States.OnUI);
            done = false;
        }
        if((Input.GetKeyDown(KeyCode.Escape) || activePanels == 0) && !done)
        {
            done = true;
            InputManager.instance.ChangeState(InputManager.States.Idle);
        }
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
    public void FocusPage(Transform t)
    {
        t.SetSiblingIndex(mails.Count);
        ShowMails();
    }

    private void ShowMails()
    {
        Vector2 pos = Vector2.zero;
        int index = 0;
        int n = quests.Count + mails.Count;
        foreach (Transform child in questFolder)
        {
            if (index >= n)
            {
                return;
            }
            child.gameObject.SetActive(true);
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
