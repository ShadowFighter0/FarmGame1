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

    private float timer;
    public float time;

    private int mailIndex = 0;
    public GameObject mailImage;

    public static MailBoxController instance;

    private void Awake()
    {
        instance = this;
        GameEvents.OnSaveInitiated += SaveMails;
    }
    private void Start()
    {
        mailsPanel = mailFolder.parent.gameObject;
        mails = Resources.LoadAll<Mail>("Data/Mails");

        if(SaveLoad.SaveExists("Mails"))
        {
            int loadIndex = SaveLoad.Load<int>("Mails");
            mailIndex = loadIndex;
        }
    }
    private void Update()
    {
        if(GameManager.instance.gameStarted)
        {
            float dt = Time.deltaTime;
            timer += dt;
            if (timer > time && mailIndex < mails.Length - 1)
            {
                timer = 0;
                mailIndex++;
                AddContent(mails[mailIndex]);
            }

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
    }
    public void SendTutorialMail()
    {
        AddContent(mails[mailIndex]);
    }
    private void SaveMails()
    {
        SaveLoad.Save(mailIndex, "Mails");
    }
    private IEnumerator CloseUIMail()
    {
        yield return new WaitForSeconds(3);
        mailImage.SetActive(false);

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

                mailImage.SetActive(true);
                StartCoroutine(CloseUIMail());
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

                mailImage.SetActive(true);
                StartCoroutine(CloseUIMail());
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
