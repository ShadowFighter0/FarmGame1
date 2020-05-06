using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    private void Awake()
    {
        instance = this;
    }

    private List<NPC> npcs = new List<NPC>();

    public void Suscribe(NPC npc) 
    {
        npcs.Add(npc);
    }

    public void AddQuest(Quest q, string name)
    {
        NPC npc = SearchNPC(name);
        if(npc != null)
        {
            SearchNPC(name).quest.Add(q);
        }
    }

    private NPC SearchNPC(string name)
    {
        foreach (NPC npc in npcs)
        {
            if(npc.name.Equals(name))
            {
                return npc;
            }
        }
        return null;
    }

    public void StartForcedDialogue(string npcName, string sentences)
    {
        NPC npc = SearchNPC(npcName);
        if (npc != null)
        {
            npc.character.GetComponent<NpcController>().ForceDialogue(sentences);
        }
    }
}
