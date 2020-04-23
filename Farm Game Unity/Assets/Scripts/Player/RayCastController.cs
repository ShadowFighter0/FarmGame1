using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    private Vector3[] hitPos = new Vector3[5];
    private GameObject[] go = new GameObject[5];

    public Transform pivot;

    public static RayCastController instance;
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        ray = new Ray(pivot.position, pivot.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit))
        {
            hitPos[0] = hit.point;
            go[0] = hit.transform.gameObject;
        }

        LayerMask mask = LayerMask.GetMask("Plants");
        if (Physics.Raycast(ray, out hit, mask))
        {
            go[1] = hit.transform.gameObject;
        }
    }

    public Vector3 HitPos() { return hitPos[0]; }
    public GameObject GetTarget() { return go[0]; }
    public GameObject GetPlant() { return go[1]; }

}