using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonController : MonoBehaviour
{
    private Image img;
    private Color32 overColor;
    private Color defaultColor;
    private AudioClip overSound;
    private AudioClip clickSound;
    private EventTrigger trigger;
    void Start()
    {
        img = GetComponent<Image>();
        CreateEvents();

        defaultColor = img.color;
        overColor = new Color32(255, 204, 0, 255);
    }

    private void CreateEvents()
    {
        trigger = GetComponent<EventTrigger>();
        if(trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry enter = new EventTrigger.Entry();

        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });

        trigger.triggers.Add(enter);

        EventTrigger.Entry exit = new EventTrigger.Entry();

        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });

        trigger.triggers.Add(exit);

        EventTrigger.Entry click = new EventTrigger.Entry();

        click.eventID = EventTriggerType.PointerClick;
        click.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });

        trigger.triggers.Add(click);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        img.color = overColor;
        //AudioManager.PlaySoundWithVariation(overSound);
    }
    public void OnPointerExit(PointerEventData data)
    {
        img.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData data)
    {
        //AudioManager.PlaySoundWithVariation(clickSound);
    }
}
