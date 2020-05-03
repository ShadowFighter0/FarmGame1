using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerController : MonoBehaviour
{
    private Transform leftChild;
    private Transform rightChild;
    private GameObject leftWater;
    private GameObject rightWater;
    private void Awake()
    {
        leftChild = transform.GetChild(0);
        leftWater = leftChild.GetChild(0).gameObject;
        leftWater.SetActive(false);

        rightChild = transform.GetChild(1);
        rightWater = rightChild.GetChild(0).gameObject;
        rightWater.SetActive(false);
    }
    private void OnEnable()
    {
        rightWater.SetActive(true);
        leftWater.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 rot = Vector3.up * Time.deltaTime * 30;
        leftChild.Rotate(rot);
        rightChild.Rotate(-rot);
    }
}
