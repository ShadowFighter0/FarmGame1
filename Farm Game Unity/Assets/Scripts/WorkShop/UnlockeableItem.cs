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

    private Material oriMat;
    private Material[] oriMats;

    private MeshRenderer mesh;
    private MeshRenderer[] meshes;

    public bool purchased = false;

    private void Awake()
    {
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
        
    }

    public void SetMaterial(Material m)
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].material = m;
            }
        }
        else
        {
            mesh.material = m;
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
