using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    private void Update()
    {
        Vector3 center = Vector3.zero;
        transform.RotateAround(center, Vector3.right, 30 * Time.deltaTime);
        transform.LookAt(center);
    }
}
