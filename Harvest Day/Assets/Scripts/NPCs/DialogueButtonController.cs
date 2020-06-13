using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    private int index;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        index = transform.GetSiblingIndex();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void Click()
    {
        DialogueSystem.instance.SendButton(index);
    }

    public void Select()
    {
        text.color = Color.yellow;
    }

    private void OnEnable()
    {
        //text.color = Color.white;
    }

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.yellow;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.white;
    }
}
