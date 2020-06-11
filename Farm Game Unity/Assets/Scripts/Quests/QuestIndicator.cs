using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestIndicator : MonoBehaviour
{
    private TextMeshProUGUI questName;
    private Coroutine coroutine;
    private CanvasGroup alpha;
    private bool coroutinePlaying = false;
    private void Start() 
    {
        questName = GetComponentInChildren<TextMeshProUGUI>();
        alpha = GetComponent<CanvasGroup>();
        alpha.alpha = 0;
    }
    public void Fill(string name)
    {
        alpha.alpha = 1;
        questName.text = name + " completed!";

        if(coroutinePlaying)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ChangeAlpha());
    }

    private IEnumerator ChangeAlpha()
    {
        coroutinePlaying = true;
        yield return new WaitForSeconds(3);
        coroutinePlaying = false;
        alpha.alpha = 0;
    }
}
