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

    private void Start()
    {
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(QuestFileInfo.Instance.GetQuest());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerNear && quests.Count > 0)
        {
            ShowMails();
            PlayerFollow.instance.SetMovement(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerFollow.instance.SetMovement(true);
        }
    }

    public void AddContent(QuestInfo q) { quests.Enqueue(q); }
    public void AddContent(Mail m) { mails.Enqueue(m); }
    public void FocusPage(Transform t)
    {
        t.SetSiblingIndex(mails.Count);
        ShowMails();
    }

    private void ShowMails()
    {
        Vector2 pos = Vector2.zero;
        int index = 0;
        foreach (Transform child in questFolder)
        {
            if(index >= quests.Count + mails.Count)
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

            }
            
            child.gameObject.GetComponent<RectTransform>().localPosition = pos;

            pos += Vector2.right * offset;
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
