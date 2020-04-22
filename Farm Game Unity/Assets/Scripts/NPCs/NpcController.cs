using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private bool playerNear;
    private bool talkStarted;

    private NPC npc;
    public string npcName = "Juana";
    public string type;

    public string[] mercaderSentences;
    private readonly KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        npc = new NPC(gameObject, npcName, type);
        NpcManager.instance.Suscribe(npc);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            bool can = DialogueSystem.instance.CanAction();

            if (playerNear && can)
            {
                if (Input.GetKeyDown(InputManager.instance.Interact) && !talkStarted)
                {
                    StartDialogue();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EndDialogue();
                }
                else if (talkStarted)
                {
                    Interact();
                }
            }
        }
    }

    /// <summary>
    /// Checks if a number has been pressed
    /// </summary>
    private void Interact()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                CheckOption(i);
            }
        }
    }

    private void StartDialogue()
    {
        talkStarted = true;
        DialogueSystem.instance.SetNPC(this);
        anim.SetBool("Talk", true);
        StartCoroutine(ChangeAnimation());

        PlayerFollow.instance.SetRotation(transform.rotation.eulerAngles + Vector3.up * 120 + Vector3.right * 20);
        PlayerFollow.instance.ChangeTarget(transform.position + Vector3.up * 1.2f);
        InputManager.instance.ChangeState(InputManager.States.Dialoguing);
        if (npc.type.Equals("Citizen"))
        {
            string s = "Need help?\n";
            DialogueSystem.instance.ShowDialogue(npcName, s, QuestToOptions());
        }
        else if (npc.type.Equals("Mercader"))
        {
            string s = "What do you want?\n";
            DialogueSystem.instance.ShowDialogue(npcName, s, SentencesToOptions());
        }
    }
    /// <summary>
    /// Converts all sentences and quest names into options to dialogue system
    /// </summary>
    /// <returns></returns>
    private string[] SentencesToOptions()
    {
        int max = mercaderSentences.Length + npc.quest.Count;
        string[] sentences = new string[max];
        int i = 0;
        foreach (string sent in mercaderSentences)
        {
            sentences[i] = sent;
            i++;
        }
        foreach (Quest q in npc.quest)
        {
            sentences[i] = q.QuestName;
            i++;
        }
        return sentences;
    }
    /// <summary>
    /// converts quest names to options
    /// </summary>
    /// <returns></returns>
    private string[] QuestToOptions()
    {
        string[] op = new string[npc.quest.Count];
        for (int i = 0; i < npc.quest.Count; i++)
        {
            op[i] = "Complete " + npc.quest[i].QuestName;
        }
        return op;
    }
    IEnumerator ChangeAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Talk", false);
    }
    private void EndDialogue()
    {
        DialogueSystem.instance.FinishDialogue();
        talkStarted = false;
        InputManager.instance.ChangeState(InputManager.States.Idle);
    }
    /// <summary>
    /// Converts the number or button pressed to an action
    /// </summary>
    /// <param name="index"></param>
    public void CheckOption(int index)
    {
        if(npc.type.Equals("Citizen"))
        {
            if (index == npc.quest.Count)
            {
                EndDialogue();
            }
            else if (index < npc.quest.Count)
            {
                UpdateQuests(index);
            }
        }
        else if(npc.type.Equals("Mercader"))
        {
            int max = npc.quest.Count + mercaderSentences.Length;
            if (index == max)
            {
                EndDialogue();
            }
            else if(index < max)
            {
                UpdateMercaderDialogue(index);

            }
        }
    }
    /// <summary>
    /// Mercader dialogues
    /// </summary>
    /// <param name="index"></param>
    private void UpdateMercaderDialogue(int index)
    {
        if (index == 0)
        {
            string sentence = "Buying";
            DialogueSystem.instance.UpdateDialogue(sentence, SentencesToOptions());
            ShopManager.Instance.GiveItems();
        }
        else if (index == 1)
        {
            string sentence = "Selling";
            DialogueSystem.instance.UpdateDialogue(sentence, SentencesToOptions());
            //Open Shop sell 
        }
        else
        {
            Quest q = npc.quest[index - mercaderSentences.Length];
            if (q != null)
            {
                if (q.Completed)
                {
                    q.GiveReward();
                    npc.quest.Remove(q);

                    string sentence = "Thank you! Take " + q.ItemReward.amount + " " + q.ItemReward.itemName;
                    DialogueSystem.instance.UpdateDialogue(sentence, SentencesToOptions());
                }
                else
                {
                    DialogueSystem.instance.UpdateDialogue(q.QuestName + " is not completed!");
                }
            }
        }
    }
    /// <summary>
    /// Citizen dialogues
    /// </summary>
    /// <param name="index"></param>
    private void UpdateQuests(int index)
    {
        Quest q = npc.quest[index];
        if (q != null)
        {
            if (q.Completed)
            {
                q.GiveReward();
                npc.quest.Remove(q);

                string sentence = "Thank you! Take " + q.ItemReward.amount + " " + q.ItemReward.itemName;
                DialogueSystem.instance.UpdateDialogue(sentence, QuestToOptions());
                anim.SetBool("Talk", true);
                StartCoroutine(ChangeAnimation());
            }
            else
            {
                DialogueSystem.instance.UpdateDialogue(q.QuestName + " is not completed!");
                anim.SetBool("Talk", true);
                StartCoroutine(ChangeAnimation());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
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
