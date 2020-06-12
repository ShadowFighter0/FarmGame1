using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioClip clickSound;
    private AudioClip overSound;
    private void Start() {
        clickSound = DataBase.GetAudioClip("MouseClick");
        overSound = DataBase.GetAudioClip("MouseOver");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(overSound);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(clickSound);
    }
}
