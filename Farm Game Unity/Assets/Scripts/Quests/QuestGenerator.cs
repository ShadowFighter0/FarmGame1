using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    private GameObject[] childs;
    Queue<QuestInfo> questsQueue = new Queue<QuestInfo>();

    public static QuestGenerator instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        childs = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            childs[i] = child.gameObject;
            i++;
        }
    }
}
