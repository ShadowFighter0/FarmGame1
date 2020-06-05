using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private MeshRenderer meshRender;

    public float maxWidth;

    public Color OutlineColor;


    private void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
    }

    public void ShowOutline()
    {
        foreach (var mat in meshRender.materials)
        {

            mat.SetFloat("_Outline", maxWidth);
            mat.SetColor("_OutlineColor", OutlineColor);
        }
    }

    public void HideOutline()
    {
        foreach (var mat in meshRender.materials)
        {
            mat.SetFloat("_Outline", 0f);
        }
    }
}
