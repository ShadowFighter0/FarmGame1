using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private int index;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
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
        DialogueSystem.instance.SendButton(index);
    }

    private void OnEnable()
    {
        text.color = Color.white;
    }
}
