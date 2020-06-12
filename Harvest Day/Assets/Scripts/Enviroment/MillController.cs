using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillController : MonoBehaviour
{
    private Transform child;
    private Vector3 rotation;
    void Start()
    {
        child = transform.GetChild(0);
        
        float randomZ = Random.Range(0.0f, 135.0f);
        rotation = new Vector3(0.0f, 0.0f, randomZ);
    }
    void Update()
    {
        child.Rotate(rotation * Time.deltaTime);
    }
}
