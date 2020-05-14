using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedsIndicator : MonoBehaviour
{
    public static SeedsIndicator instance;
    private Transform child;
    private int lastActive;
    private void Awake()
    {
        instance = this;
        child = transform.GetChild(0);
    }

    private void Update()
    {
        child.Rotate(Vector3.up * 120 * Time.deltaTime);

        Vector3 newPos;
        newPos.x = child.position.x;
        newPos.y = Mathf.Sin(Time.time * 3) / 10 + 1;
        newPos.z = child.position.z;

        child.position = newPos;
    }
    public void ChangePosition(Vector3 newPos)
    {
        transform.position = newPos;
    }
    public void ActivateChild(string name)
    {
        foreach (Transform child in child)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }
        foreach (Transform child in child)
        {
            if (child.gameObject.name.Equals(name))
            {
                child.gameObject.SetActive(true);
            }
        }
    }

}
