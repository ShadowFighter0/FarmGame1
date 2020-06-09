using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Material[] mats;
    private float maxWidth = 0.06f;
    private Color OutlineColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    public Shader shader;

    public bool isWorldObject;

    private void Awake()
    {
        MeshRenderer meshRender = GetComponent<MeshRenderer>();
         mats = new Material[meshRender.materials.Length];
        for(int i = 0; i < mats.Length; i++)
        {
            mats[i] = new Material(meshRender.materials[i]);
            mats[i].shader = shader;
        }
        meshRender.materials = mats;
        GameEvents.OnTutorialDone += TutorialDone;
        ShowOutline();
    }
    private void TutorialDone()
    {
        if (isWorldObject)
        {
            HideOutline();
        }
    }

    public void ShowOutline()
    {
        foreach (Material mat in mats)
        {
            mat.SetFloat("_Outline", maxWidth);
            mat.SetColor("_OutlineColor", OutlineColor);
        }
    }

    public void HideOutline()
    {
        foreach (Material mat in mats)
        {
            mat.SetFloat("_Outline", 0f);
        }
    }
}
