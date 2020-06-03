using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    private void Awake()
    {
        instance = this;
    }
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private TextMeshProUGUI[] options;

    private RectTransform rect;
    private float iniY;

    private bool firstDialogue;

    private bool canAction = true;
    private bool sentenceFinished = true;

    private NpcController current;

    private Vector3 pos;

    private void Start()
    {
        options = new TextMeshProUGUI[transform.GetChild(1).childCount];
        int i = 0;
        foreach (Transform child in transform.GetChild(1))
        {
            options[i] = child.GetComponent<TextMeshProUGUI>();
            options[i].gameObject.SetActive(false);
            i++;
        }
        rect = GetComponent<RectTransform>();
        iniY = rect.localPosition.y;
        pos = rect.localPosition;
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

    private void Update()
    {
        rect.localPosition = Vector3.Lerp(rect.localPosition, pos, Time.deltaTime * 10.0f);
    }

    public void ShowDialogue(string name, string dialogue, string[] options)
    {
        Vector3 newPos;
        newPos.x = rect.localPosition.x;
        newPos.y = -350;
        newPos.z = rect.localPosition.z;

        pos = newPos;

        firstDialogue = true;
        InputManager.instance.ChangeState(InputManager.States.Dialoguing);

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
        pos = rect.localPosition + Vector3.up * iniY;

        StartCoroutine(Action());
        SetNPC(null);
        InputManager.instance.ChangeState(InputManager.States.Dialoguing);
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
