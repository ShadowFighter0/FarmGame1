using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    private void Awake()
    {
        instance = this;
    }
    private Text nameText;
    private Text dialogueText;

    private Text[] options;

    private RectTransform rect;
    private float iniY;

    private bool firstDialogue;

    private bool canAction = true;
    private bool sentenceFinished = true;

    private NpcController current;

    private void Start()
    {
        nameText = transform.GetChild(0).GetComponent<Text>();
        dialogueText = transform.GetChild(1).GetComponent<Text>();
        options = new Text[transform.GetChild(2).childCount];
        int i = 0;
        foreach (Transform child in transform.GetChild(2))
        {
            options[i] = child.GetComponent<Text>();
            options[i].gameObject.SetActive(false);
            i++;
        }
        rect = GetComponent<RectTransform>();
        iniY = rect.localPosition.y;
    }
    public void SetNPC(NpcController npc) { current = npc; }
    public bool CanAction() { return canAction; }

    public void SendButton(int i) 
    { 
        if(current != null)
        {
            current.CheckOption(i);
        }
    }

    public void ShowDialogue(string name, string dialogue, string[] options)
    {
        LeanTween.moveLocalY(gameObject, rect.localPosition.y + 450, 0.5f).setEaseOutSine();
        firstDialogue = true;
        MovementController.instance.SetMovement(false);
        PlayerFollow.instance.SetMovement(false);

        SetName(name);
        SetDialogue(dialogue);
        SetOptions(options);
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        canAction = false;
        yield return new WaitForSeconds(0.5f);
        canAction = true;
    }

    public void FinishDialogue()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].gameObject.SetActive(false);
        }
        LeanTween.moveLocalY(gameObject, iniY, 0.5f).setEaseInSine();

        StartCoroutine(Action());
        SetNPC(null);

        MovementController.instance.SetMovement(true);
        PlayerFollow.instance.SetMovement(true);
    }
    public void UpdateDialogue(string sentence, string[] op)
    {
        if(sentenceFinished)
        {
            SetDialogue(sentence);
            SetOptions(op);
        }
    }

    public void UpdateDialogue(string sentence)
    {
        if (sentenceFinished)
        {
            SetDialogue(sentence);
        }
    }

    private void SetName(string name) { nameText.text = name; }
    private void SetDialogue(string dialogue) 
    {
        dialogueText.text = "";
        StartCoroutine(DialogueDelay(dialogue));
    }
    IEnumerator DialogueDelay(string sentence)
    {
        sentenceFinished = false;
        if (firstDialogue)
        {
            firstDialogue = false;
            yield return new WaitForSeconds(0.5f);
        }
        
        foreach (char c in sentence)
        {
            yield return null;
            dialogueText.text += c;
        }
        sentenceFinished = true;
    }
    private void SetOptions(string[] opt)
    {
        int last = opt.Length;
        for (int i = 0; i < options.Length; i++)
        {
            if (i <= last)
            {
                options[i].gameObject.SetActive(true);
                if (i == last)
                {
                    options[i].text = (i + 1) + ".- Exit";
                }
                else
                {
                    options[i].text = (i + 1) + ".- " + opt[i];
                }
            }
            else
            {
                options[i].gameObject.SetActive(false);
            }
        }
    }
}
