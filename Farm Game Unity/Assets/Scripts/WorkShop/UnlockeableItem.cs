using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnlockeableItem : MonoBehaviour
{
    public string itemName;
    public string description;
    public Item[] requirements;
    public int[] amounts;
    public string iniTag;

    public Material mat;

    private Material oriMat;
    private Material[] oriMats;

    private MeshRenderer mesh;
    private MeshRenderer[] meshes;

    public bool purchased = false;

    private void Awake()
    {
        iniTag = gameObject.tag;
        gameObject.tag = "UnlockableItem";
        if(transform.childCount > 0)
        {
            meshes = GetComponentsInChildren<MeshRenderer>();
            oriMats = new Material[meshes.Length];
            for (int i = 0; i < meshes.Length; i++)
            {
                oriMats[i] = meshes[i].material;
            }
        }
        else
        {
            mesh = GetComponent<MeshRenderer>();
            oriMat = mesh.material;
        }
    }

    public void Purchased()
    {
        purchased = true;
        gameObject.tag = iniTag;
    }

    public void SetMaterial()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].material = mat;
            }
        }
        else
        {
            mesh.material = mat;
        }
    }

    public void SetOriginalMat()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].material = oriMats[i];
            }
        }
        else
        {
            mesh.material = oriMat;
        }
    }
}
