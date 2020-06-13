using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    private AudioClip clickSound;
    private AudioClip overSound;
    public Color main;
    public bool byHand = true;

    private void Awake()
    {
        if (gameObject.GetComponent<Image>() != null && !byHand)
            main = gameObject.GetComponent<Image>().color;
    }
    private void Start() {
        clickSound = DataBase.GetAudioClip("MouseClick");
        overSound = DataBase.GetAudioClip("MouseOver");
    }

    private void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    public void SaveData()
    {
        main = gameObject.GetComponent<Image>().color;
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = new Color32(255, 204, 0, 255);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(overSound);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.PlaySoundWithVariation(clickSound);

        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = main;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (overSound!= null)
            AudioManager.PlaySoundWithVariation(overSound);
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = new Color32(255, 204, 0, 255);

  
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (gameObject.GetComponent<Image>() != null)
            gameObject.GetComponent<Image>().color = main;

        Debug.Log(gameObject.GetComponent<Image>());
    }
}
