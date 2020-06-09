using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Image img;
    private Color overColor;
    private Color defaultColor;
    private AudioClip overSound;
    private AudioClip clickSound;
    void Start()
    {
        img = GetComponent<Image>();
        defaultColor = img.color;
        overColor = new Color(255/ 2555, 153/255, 0);
    }
    public void OnPointerEnter()
    {
        img.color = overColor;
        AudioManager.PlaySoundWithVariation(overSound);
    }
    public void OnPointerExit()
    {
        img.color = defaultColor;
    }

    public void OnPointerClick()
    {
        AudioManager.PlaySoundWithVariation(clickSound);
    }
}
