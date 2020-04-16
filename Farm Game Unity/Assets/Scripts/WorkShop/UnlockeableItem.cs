using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockeableItem : MonoBehaviour
{
    public string itemName;
    public string description;
    public Item[] requirements;
    public int[] amounts;

    private Material oriMat;
    private MeshRenderer mesh;

    public bool purchased = false;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material m)
    {
        mesh.material = m;
    }

    public void SetOriginalMat()
    {
        mesh.material = oriMat;
    }
}
