using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
    public GameObject character;
    public List<Quest> quest = new List<Quest>();
    public string name;
    public string type;

    public NPC(GameObject go, string n, string t)
    {
        character = go;
        name = n;
        type = t;
    }
}
