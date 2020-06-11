using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxController : MonoBehaviour
{
    private bool playerNear;
    public Transform mailFolder;
    private GameObject mailsPanel;
    public int offset = 70;

    private Mail[] mails;
    private QuestTemplate[] quests;

    private bool done = true;

    private float timer;
    public float time;

    private int mailIndex = -2;
    public GameObject mailImage;

    public AudioClip open;
    private AudioClip mailReceived;

    public static MailBoxController instance;

    private bool tutorialDone = false;

    private List<GameObject> activePanels = new List<GameObject>();

    public Mail tutorialMail;
    public Mail shopMail;
    private OutlineController outline;
    private bool firstMails;
    private void Awake()
    {
        instance = this;
        GameEvents.OnSaveInitiated += SaveMails;
        GameEvents.OnTutorialDone += TutorialDone;
    }
    private void Start()
    {
        mailReceived = DataBase.GetAudioClip("MailNotification");
        mailsPanel = mailFolder.parent.gameObject;
        mails = Resources.LoadAll<Mail>("Data/Mails");
        quests = Resources.LoadAll<QuestTemplate>("Data/Mails");
        if(SaveLoad.SaveExists("Mails"))
        {
            int loadIndex = SaveLoad.Load<int>("Mails");
            mailIndex = loadIndex;
        }
        outline = GetComponent<OutlineController>();
    }
    private void Update()
    {
        if(GameManager.instance.gameStarted)
        {
            int currentPanels = activePanels.Count;
            if (tutorialDone)
            {
                float dt = Time.deltaTime;
                timer += dt;
                if (timer > time && done && mailIndex < quests.Length - 1 && currentPanels < 3)
                {
                    timer = 0;
                    AddContent(new QuestInfo(quests[mailIndex]));
                }
            }

            if (playerNear)
            {
                if (Input.GetKeyDown(InputManager.instance.Interact) && done)
                {
                    mailsPanel.SetActive(true);
                    //AudioManager.PlaySoundWithVariation(open);
                    InputManager.instance.ChangeState(InputManager.States.OnUI);
                    done = false;
                }
                if ((Input.GetKeyDown(KeyCode.F1) || activePanels.Count == 0) && !done)
                {
                    AudioManager.PlaySoundWithVariation(open);
                    Close();
                }
            }
        }
    }
    private void TutorialDone()
    {
        tutorialDone = true;
        firstMails = true;
    }
    public void SendTutorialMail()
    {
        AddContent(tutorialMail);
        AddContent(shopMail);
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
        if(!firstMails)
        {
            firstMails = true;
            TutorialController.instance.SetThingDone(0);
            TutorialController.instance.SendWorkshopMessage();
        }
    }

    public void AddContent(QuestInfo info)
    {
        foreach (Transform child in mailFolder)
        {
            if(!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.SetAsFirstSibling();

                activePanels.Add(child.gameObject);

                child.GetComponent<MailBoxPanel>().Assign(info);

                mailImage.SetActive(true);
                StartCoroutine(CloseUIMail());
                AudioManager.PlaySound(mailReceived);
                mailIndex++;
                return;
            }
        }
    }
    public void RemovePanel(GameObject go)
    {
        activePanels.Remove(go);
    }
    public void AddContent(Mail info)
    {
        foreach (Transform child in mailFolder)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                child.SetAsFirstSibling();

                activePanels.Add(child.gameObject);

                child.GetComponent<MailBoxPanel>().Assign(info);

                mailImage.SetActive(true);
                StartCoroutine(CloseUIMail());
                AudioManager.PlaySound(mailReceived);
                mailIndex++;
                return;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            outline.ShowOutline();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            outline.HideOutline();
        }
    }
}
