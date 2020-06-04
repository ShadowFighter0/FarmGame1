using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialTrigger : MonoBehaviour
{
    public VideoClip video;
    [TextArea]
    public string description;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            TutorialController.instance.TutorialPopUp(video, description);
            gameObject.SetActive(false);
            TutorialTriggersController.instance.SaveTriggers();
        }
    }
}
