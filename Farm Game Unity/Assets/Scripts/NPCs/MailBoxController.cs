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

    public AudioClip open;
    private AudioClip mailReceived;

    private int activeMails = 0;

    public static MailBoxController instance;

    private void Awake()
    {
        instance = this;
        GameEvents.OnSaveInitiated += SaveMails;
    }
    private void Start()
    {
        mailReceived = DataBase.GetAudioClip("MailNotification");
        mailsPanel = mailFolder.parent.gameObject;
        mails = Resources.LoadAll<Mail>("Data/Mails");

        if(SaveLoad.SaveExists("Mails"))
        {
            int loadIndex = SaveLoad.Load<int>("Mails");
            mailIndex = loadIndex;
        }
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(QuestFileInfo.Instance.GetQuest());
        AddContent(QuestFileInfo.Instance.GetQuest());
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
                if (Input.GetKeyDown(InputManager.instance.Interact) && done && activeMails > 0)
                {
                    mailsPanel.SetActive(true);
                    AudioManager.PlaySoundWithVariation(open);
                    InputManager.instance.ChangeState(InputManager.States.OnUI);
                    UpdatePositions();
                    done = false;
                }
                if ((Input.GetKeyDown(KeyCode.F1) || activeMails == 0) && !done)
                {
                    AudioManager.PlaySoundWithVariation(open);
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

    private void UpdatePositions()
    {
        int offset = 20;
        int index = 0;
        foreach (Transform child in mailFolder)
        {
            child.GetComponent<RectTransform>().localPosition += Vector3.right * offset * index;
            index++;
        }
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
                AudioManager.PlaySound(mailReceived);
                activeMails++;
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
                AudioManager.PlaySound(mailReceived);
                activeMails++;
                return;
            }
        }
    }

    public void TakeMail()
    {
        int i = activeMails - 1;
        mailFolder.GetChild(i).GetComponent<MailBoxPanel>().AddContent();
        activeMails--;
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
