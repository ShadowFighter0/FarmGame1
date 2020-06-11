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

    }
    private void Start() {
        if (SaveLoad.SaveExists("ThingsDone"))
        {
            thingsDone = SaveLoad.Load<bool[]>("ThingsDone");
            SendEvent();
        }
    }
    private void SendEvent()
    {
        if (TutorialDone())
        {
            GameEvents.TutorialDone();
            Debug.Log("Tutorial done!");
        }
    }

    public void SetThingDone(int i) 
    { 
        thingsDone[i] = true;
        SendEvent();
    }
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
        NpcManager.instance.StartForcedDialogue("Maria", "Hi! I'm Maria. Welcome to Harvest Day. In the mailbox you will find your first order!");
        MailBoxController.instance.SendTutorialMail();
    }

    public void SendWorkshopMessage()
    {
        NpcManager.instance.StartForcedDialogue("Maria", "Now you need an orchard and seeds. Go to the barn and buy one with the workshop!");
    }
    public void SendOrdersTutorial()
    {
        NpcManager.instance.StartForcedDialogue("Maria", "That's an order. When you have completed it, go to the car next to me and complete it");
    }
    public void SendShopMessage()
    {
        NpcManager.instance.StartForcedDialogue("Pepe", "In the shop you can buy the seeds that you need. Select the seeds that you want and talk with me to confirm the purchase. You can see the seeds that you have in your cart in the cart button.");
    }

    private bool TutorialDone()
    {
        for (int i = 0; i < thingsDone.Length; i++)
        {
            if(!thingsDone[i])
            {
                return false;
            }
        }
        return true;
    }
}
