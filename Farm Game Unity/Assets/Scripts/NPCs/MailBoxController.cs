using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

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

    private int mailIndex = 0;
    public GameObject mailImage;

    public AudioClip open;
    private AudioClip mailReceived;

    public static MailBoxController instance;

    private bool tutorialDone = false;

    private List<GameObject> activePanels = new List<GameObject>();
    private int panelIndex = 0;

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
                if (timer > time && done && currentPanels < 3)
                {
                    timer = 0;
                    AddContent(new QuestInfo(quests[mailIndex]));
                }
            }

            if (playerNear)
            {
                if (Input.GetKeyDown(InputManager.instance.Interact) && done && currentPanels > 0)
                {
                    panelIndex = 0;
                    mailsPanel.SetActive(true);
                    AudioManager.PlaySoundWithVariation(open);
                    InputManager.instance.ChangeState(InputManager.States.OnUI);
                    UpdatePositions();
                    done = false;
                }
                if ((Input.GetKeyDown(KeyCode.F1) || currentPanels == 0) && !done)
                {
                    AudioManager.PlaySoundWithVariation(open);
                    Close();

                    if (!TutorialController.instance.GetThingDone(0))
                    {
                        TutorialController.instance.SendWorkshopMessage();
                        TutorialController.instance.SetThingDone(0);
                    }
                }
            }
        }
    }
    private void TutorialDone()
    {
        tutorialDone = true;
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

    public void MovePanels(int dir)
    {
        int newIndex = panelIndex + dir;
        if(newIndex >= 0 && newIndex <= activePanels.Count - 1)
        {
            panelIndex = newIndex;
            activePanels[panelIndex].transform.SetAsLastSibling();
        }
    }

    public void AddContent(QuestInfo info)
    {
        foreach (Transform child in mailFolder)
        {
            if(!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);

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
    public void AddContent(Mail info)
    {
        foreach (Transform child in mailFolder)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);

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

    public void TakeMail()
    {
        if(activePanels.Count > 0)
        {
            mailFolder.GetChild(panelIndex).GetComponent<MailBoxPanel>().AddContent();
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
