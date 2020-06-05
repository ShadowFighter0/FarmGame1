using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Material mat;
    private float maxWidth = 0.06f;
    private Color OutlineColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    public Shader shader;

    private void Start()
    {
        MeshRenderer meshRender = GetComponent<MeshRenderer>();
        mat = new Material(meshRender.material);
        mat.shader = shader;
        meshRender.material = mat;

        ShowOutline();
    }

    public void ShowOutline()
    {
        mat.SetFloat("_Outline", maxWidth);
        mat.SetColor("_OutlineColor", OutlineColor);
    }

    public void HideOutline()
    {
        mat.SetFloat("_Outline", 0f);
    }
}
