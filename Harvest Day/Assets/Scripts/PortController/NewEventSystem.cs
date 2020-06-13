using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewEventSystem : MonoBehaviour
{
    public GameObject button;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void AssignNewSelected(GameObject aux)
    {
        EventSystem.current.SetSelectedGameObject(aux);
    }
}
