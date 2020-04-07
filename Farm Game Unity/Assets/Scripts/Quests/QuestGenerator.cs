using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour, INewDay
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

    public void NewDay()
    {
        AddMissions(2);
    }
    public void AddMissions(int n)
    {
        int index = 0;
        foreach (GameObject go in childs)
        {
            if(questsQueue.Count > 0)
            {
                QuestInfo q = questsQueue.Dequeue();
                go.SetActive(true);
                go.GetComponent<QuestPanel>().AssignQuest(q);
            }
            else
            {
                if (index >= n)
                {
                    return;
                }

                if (!go.activeSelf)
                {
                    QuestInfo q = QuestFileInfo.Instance.GetQuest();
                    go.SetActive(true);
                    go.GetComponent<QuestPanel>().AssignQuest(q);
                    index++;
                }
            }
        }
        if(index == 0)
        {
            for (int i = 0; i < n; i++)
            {
                questsQueue.Enqueue(QuestFileInfo.Instance.GetQuest());
            }
        }
    }
}
