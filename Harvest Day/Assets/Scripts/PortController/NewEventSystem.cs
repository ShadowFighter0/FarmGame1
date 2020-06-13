using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewEventSystem : MonoBehaviour
{
    public GameObject button;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}
