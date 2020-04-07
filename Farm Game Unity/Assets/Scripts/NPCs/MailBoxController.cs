using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxController : MonoBehaviour
{
    private bool playerNear;
    private List<Mail> mails = new List<Mail>();

    public GameObject[] UIMails;
    public int offset = 70;
    public Mail[] mail;
    public void AddMail(Mail m)
    {
        mails.Add(m);
    }
    private void Start()
    {
        AddMail(mail[0]);
        AddMail(mail[1]);
        AddMail(mail[2]);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerNear && mails.Count > 0)
        {
            ShowMails();
            PlayerFollow.instance.SetMovement(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerFollow.instance.SetMovement(true);
        }
    }

    public void TakeMail(int i)
    {
        Quest q = mails[i].quest;
        if (q != null)
        {
            QuestController.Instance.AddQuest(q);
        }
        Item item = mails[i].rewardItem;
        if(item != null)
        {
            InventoryController.Instance.AddItem(item);
        }
        mails.RemoveAt(i);
        if(mails.Count == 0)
        {
            PlayerFollow.instance.SetMovement(true);
        }
        ShowMails();
    }

    public void FocusPage(Transform t)
    {
        t.SetSiblingIndex(mails.Count);
        ShowMails();
    }

    private void ShowMails()
    {
        int i = mails.Count - 1;
        Vector2 pos = Vector2.zero;
        foreach (Mail m in mails)
        {
            UIMails[i].SetActive(true);
            UIMails[i].GetComponentInChildren<Text>().text = m.message;

            UIMails[i].GetComponent<RectTransform>().localPosition = pos;
            pos += Vector2.right * offset;
            i--;
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
