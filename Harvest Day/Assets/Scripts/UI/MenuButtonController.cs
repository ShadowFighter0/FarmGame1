using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    private AudioClip clickSound;
    private AudioClip overSound;
    private Color main;

    private void Start() {
        clickSound = DataBase.GetAudioClip("MouseClick");
        overSound = DataBase.GetAudioClip("MouseOver");

        if(gameObject.GetComponent<Image>() != null)
            main = gameObject.GetComponent<Image>().color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(overSound);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(clickSound);

        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = main;
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(overSound);
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = new Color32(255, 204, 0, 255);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = main;
    }

    public void RestartColor()
    {
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = main;
    }
}
