using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTextController : MonoBehaviour
{
    public static ActionTextController instance;
    private void Awake()
    {
        instance = this;
    }
    public Transform player;
    private TextMesh text;
    private void Start()
    {
        text = GetComponent<TextMesh>();
    }
    private void Update()
    {
        transform.LookAt(player);
        transform.eulerAngles += Vector3.up * 180;
    }

    public void ChangeText(string s)
    {
        text.text = s;
    }
    public void ChangePosition(Vector3 newPos)
    {
        transform.position = newPos + Vector3.up * 1;
    }
}
