using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestEntry : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private Quest quest;

    private void Awake()
    {
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void Fill(string name, string description)
    {
        nameText.text = name;
        descriptionText.text = description;
    }
    public void AssignQuest(Quest q)
    {
        quest = q;
    }
    public void Button()
    {
        QuestController.Instance.OnRemoveQuestButton(quest);
    }
}
