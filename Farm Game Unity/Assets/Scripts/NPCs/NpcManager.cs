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
        Debug.Log(npc.name + " added");
    }

    public void AddQuest(Quest q, string name)
    {
        NPC npc = SearchNPC(name);
        if(npc != null)
        {
            Debug.Log(npc.name + " find");
            SearchNPC(name).quest.Add(q);
            Debug.Log(q.QuestName + " added to " + npc.name);
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
}
