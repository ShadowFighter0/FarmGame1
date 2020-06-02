using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceBarController : MonoBehaviour
{
    private CanvasGroup group;
    private Coroutine co;
    private bool coPlaying = false;

    public static ExperienceBarController instace;
    private void Awake()
    {
        instace = this;
    }
    void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }

    public void ShowBar()
    {
        group.alpha = 1;
        if (coPlaying)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(Close());
    }

    private void CloseBar()
    {
        group.alpha = 0;
    }
    IEnumerator Close()
    {
        coPlaying = true;
        yield return new WaitForSeconds(3);
        coPlaying = false;
        CloseBar();
    }
}
