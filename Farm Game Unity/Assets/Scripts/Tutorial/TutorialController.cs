using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    private int state = -1;
    public GameObject popUp;
    public TextMeshProUGUI text;
    public VideoPlayer player;
    public static TutorialController instance;
    private void Awake()
    {
        instance = this;
    }

    public void NextState()
    {
        state++;
        UpdateStates();
    }

    public void TutorialPopUp(VideoClip video, string description)
    {
        popUp.SetActive(true);
        text.text = description;
        player.clip = video;
        player.Play();
        InputManager.instance.ChangeState(InputManager.States.OnUI);
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }

    private void UpdateStates()
    {
        switch (state)
        {
            case 0:
                NpcManager.instance.StartForcedDialogue("Maria", "Hi! I'm Maria. In the mailbox next to me you will find your first order!\nCome back to me when you're done");
                MailBoxController.instance.SendTutorialMail();
                PlayerManager.instace.SendCarrots();
                break;
            case 1:
                NpcManager.instance.StartForcedDialogue("Maria", "Now you need an orchard. Go to the barn and buy one!");
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
