using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void OnEnable()
    {
        StartCoroutine(MoveChild());
    }

    IEnumerator MoveChild()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
