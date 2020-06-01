using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    public GameObject options;
    public CanvasGroup canvas;
    public void OnClick()
    {
        bool b = !options.activeSelf;
        options.SetActive(b);
        if(b)
        {
            StartCoroutine(ChangeAlpha(1));
        }
        else
        {
            StartCoroutine(ChangeAlpha(0));
        }
    }

    private IEnumerator ChangeAlpha(int alpha)
    {
        bool exit = false;
        while (!exit)
        {
            yield return null;
            canvas.alpha = Mathf.Lerp(canvas.alpha, alpha, Time.deltaTime * 0.1f);
            if(Mathf.Approximately(canvas.alpha, alpha))
            {
                exit = true;
            }
        }
    }
}
