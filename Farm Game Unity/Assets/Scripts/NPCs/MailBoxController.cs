using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxController : MonoBehaviour
{
    private bool playerNear;

    public Transform mailFolder;
    private GameObject mailsPanel;
    public int offset = 70;

    private Mail[] mails;

    private bool done = true;

    private void Start()
    {
        mailsPanel = mailFolder.parent.gameObject;

        mails = Resources.LoadAll<Mail>("Data/Mails");

        AddContent(mails[0]);
    }
    private void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(InputManager.instance.Interact) && done)
            {
                mailsPanel.SetActive(true);

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

    public void AddContent(QuestInfo info)
    {
        foreach (Transform child in mailFolder)
        {
            if(!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<MailBoxPanel>().Assign(info);
                return;
            }
        }
    }
    public void AddContent(Mail info)
    {
        foreach (Transform child in mailFolder)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<MailBoxPanel>().Assign(info);
                return;
            }
        }
    }

    public void TakeMail()
    {
        int i = GetActiveMails() - 1;
        mailFolder.GetChild(i).GetComponent<MailBoxPanel>().AddContent();
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
