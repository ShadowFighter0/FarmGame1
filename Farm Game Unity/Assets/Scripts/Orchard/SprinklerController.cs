using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerController : MonoBehaviour
{
    private Transform leftChild;
    private Transform rightChild;

    private Transform[] leftChildPivots;
    private Transform[] rightChildPivots;

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

        leftChildPivots = new Transform[leftChild.childCount];
        for (int i = 0; i < leftChildPivots.Length; i++)
        {
            leftChildPivots[i] = leftChild.GetChild(i);
        }

        rightChildPivots = new Transform[rightChild.childCount];
        for (int i = 0; i < leftChildPivots.Length; i++)
        {
            rightChildPivots[i] = rightChild.GetChild(i);
        }
    }
    private void OnEnable()
    {
        rightWater.SetActive(true);
        leftWater.SetActive(true);
    }

    void Update()
    {
        Vector3 rot = Vector3.up * Time.deltaTime * 30;
        leftChild.Rotate(rot);
        rightChild.Rotate(-rot);

        for (int i = 0; i < leftChildPivots.Length; i++)
        {
            Ray ray = new Ray(leftChildPivots[i].position, Vector3.down);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.transform.gameObject.CompareTag("Hole"))
                {
                    hit.transform.GetComponent<HoleController>().AddWater(100.0f);
                }
            }
        }

        for (int i = 0; i < rightChildPivots.Length; i++)
        {
            Ray ray = new Ray(rightChildPivots[i].position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.CompareTag("Hole"))
                {
                    hit.transform.GetComponent<HoleController>().AddWater(100.0f);
                }
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject);
    }
}
