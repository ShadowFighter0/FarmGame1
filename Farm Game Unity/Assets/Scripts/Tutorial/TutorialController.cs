using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    public GameObject popUp;
    public TextMeshProUGUI text;
    public VideoPlayer player;
    public static TutorialController instance;

    private bool[] thingsDone = new bool[3];
    //0 -> first mail
    //1 -> orchard bought
    //2 -> first quest completed

    private void Awake()
    {
        instance = this;
        if (SaveLoad.SaveExists("ThingsDone"))
        {
            thingsDone = SaveLoad.Load<bool[]>("ThingsDone");
        }
    }

    public void SetThingDone(int i) { thingsDone[i] = true; }
    public bool GetThingDone(int i) { return thingsDone[i]; }
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

    public void Save()
    {
        SaveLoad.Save(thingsDone, "ThingsDone");
    }

    public void SendFirstMail()
    {
        NpcManager.instance.StartForcedDialogue("Maria", "Hi! I'm Maria. In the mailbox next to me you will find your first order!\nCome back to me when you're done");
        MailBoxController.instance.SendTutorialMail();
        PlayerManager.instace.SendCarrots();
    }

    public void SendWorkshopMessage()
    {
        NpcManager.instance.StartForcedDialogue("Maria", "Now you need an orchard. Go to the barn and buy one with the workshop!");
    }
    public void SendShopMessage()
    {
        NpcManager.instance.StartForcedDialogue("Maria", "kghjhjhg!");
    }
}
