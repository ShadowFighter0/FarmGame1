using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedController : MonoBehaviour
{
    private Queue<FeedInfo> queue;
    private Queue<GameObject> feed;
    private List<GameObject> activePanels;
    private Vector2 oriPosition;

    private float timer;
    private float deactivateTime = 2;

    void Start()
    {
        feed = new Queue<GameObject>();
        queue = new Queue<FeedInfo>();
        activePanels = new List<GameObject>();
        oriPosition = transform.GetChild(0).GetComponent<RectTransform>().localPosition;

        foreach (Transform child in transform)
        {
            feed.Enqueue(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer < 0 && activePanels.Count > 0)
        {
            RemovePanel();
        }
        AddToQueue();
        MovePanels();
    }

    private void RemovePanel()
    {
        GameObject go = activePanels[0];
        activePanels.RemoveAt(0);

        go.GetComponent<RectTransform>().localPosition = oriPosition;
        go.SetActive(false);
        timer = deactivateTime;
    }

    public void Suscribe(string tag, Sprite image, int amount)
    {
        FeedInfo newPanel = new FeedInfo(tag, image, amount);
        AddFeed(newPanel);
    }

    public void MovePanels()
    {
        Vector2 offset;
        for (int i = 0; i < activePanels.Count; i++)
        {
            offset.y = (activePanels.Count - 1 - i) * 100;
            offset.x = 170;

            RectTransform rect = activePanels[i].GetComponent<RectTransform>();
            rect.localPosition = Vector3.Lerp(rect.localPosition, offset, 10 * Time.deltaTime);
        }
    }

    private void AddToQueue()
    {
        if (queue.Count > 0 && activePanels.Count < feed.Count)
        {
            PanelInfo(queue.Dequeue());
        }
    }

    private void AddFeed(FeedInfo info)
    {
        if (activePanels.Count < feed.Count)
        {
            PanelInfo(info);
        }
        else
        {
            queue.Enqueue(info);
        }
    }

    private void PanelInfo(FeedInfo info)
    {
        timer = deactivateTime;
        GameObject last = FindWithTag(info.Tag);
        if (last != null)
        {
            string text = last.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            text = text.Replace("x", string.Empty);
            int.TryParse(text, out int amount);
            amount += info.Amount;
            last.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + amount.ToString();
        }
        else
        {
            GameObject go = feed.Dequeue();
            activePanels.Add(go);
            go.GetComponent<Image>().sprite = info.Image;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + info.Amount.ToString();
            go.SetActive(true);
            go.GetComponent<FeedID>().SetID(info.Tag);

            feed.Enqueue(go);
        }
    }

    private GameObject FindWithTag(string tag)
    {
        foreach (GameObject go in activePanels)
        {
            if(go.GetComponent<FeedID>().GetID() == tag)
            {
                return go;
            }
        }
        return null;
    }
}

public class FeedInfo
{
    public FeedInfo(string tg, Sprite im, int a)
    {
        Tag = tg;
        Image = im;
        Amount = a;
    }
    public FeedInfo()
    {
        Tag = null;
        Image = null;
        Amount = 0;
    }

    public FeedInfo(FeedInfo f)
    {
        Tag = f.Tag;
        Image = f.Image;
        Amount = f.Amount;
    }

    public Sprite Image { get; }
    public string Tag { get; }
    public int Amount { get; private set; }
}