using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Image img;
    private Color32 overColor;
    private Color defaultColor;
    private AudioClip overSound;
    private AudioClip clickSound;
    void Start()
    {
        img = GetComponent<Image>();
        overSound = DataBase.GetAudioClip("MouseOver");
        clickSound = DataBase.GetAudioClip("MouseClick");
        defaultColor = img.color;
        overColor = new Color32(255, 204, 0, 255);
    }
    public void OnPointerEnter(PointerEventData data)
    {
        img.color = overColor;
        AudioManager.PlaySoundWithVariation(overSound);
    }
    public void OnPointerExit(PointerEventData data)
    {
        img.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData data)
    {
        AudioManager.PlaySoundWithVariation(clickSound);
    }
    public void OnSelect(BaseEventData eventData)
    {
        if(gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = overColor;

        if (overSound != null)
            AudioManager.PlaySoundWithVariation(overSound);

        Debug.Log(gameObject.name);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = defaultColor;
    }
}
