using TMPro;
using UnityEngine;

public class QuestEntry : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private Quest quest;

    private void Awake()
    {
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void Fill(string name, string description)
    {
        nameText.text = name;
        descriptionText.text = description;
    }
    public void Fill()
    {
        nameText.text = "";
        descriptionText.text = "";
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
