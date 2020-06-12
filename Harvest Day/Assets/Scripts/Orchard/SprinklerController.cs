using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerController : MonoBehaviour
{
    private Transform leftChild;
    private Transform rightChild;

    private Transform[] leftChildPivots;
    private Transform[] rightChildPivots;

    private ParticleSystem leftWater;
    private ParticleSystem rightWater;

    private float time;
    private float rotationTime;
    private const float anglesPerSecond = 30f;

    private bool canRotate = false;
    private void Awake()
    {
        leftChild = transform.GetChild(0);
        leftWater = leftChild.GetChild(0).GetComponent<ParticleSystem>();
        leftWater.Stop();

        rightChild = transform.GetChild(1);
        rightWater = rightChild.GetChild(0).GetComponent<ParticleSystem>();
        rightWater.Stop();

        leftChildPivots = new Transform[leftChild.childCount - 1];
        for (int i = 0; i < leftChildPivots.Length; i++)
        {
            leftChildPivots[i] = leftChild.GetChild(i + 1);
        }

        rightChildPivots = new Transform[rightChild.childCount - 1];
        for (int i = 0; i < leftChildPivots.Length; i++)
        {
            rightChildPivots[i] = rightChild.GetChild(i + 1);
        }
    }
    private void OnEnable()
    {
        if(GetComponent<UnlockeableItem>().purchased)
        {
            time = TimeManager.instance.secondsPerDay / 12;
            rotationTime = 360.0f / anglesPerSecond;
            StartCoroutine(ActivateWater());
        }
    }

    void Update()
    {
        if(canRotate)
        {
            Vector3 rot = Vector3.up * Time.deltaTime * anglesPerSecond;
            leftChild.Rotate(rot);
            rightChild.Rotate(-rot);
        }
    }

    private IEnumerator DoWater()
    {
        int index = 0;
        SetActive(true);

        do {
            LaunchRayCasts(leftChildPivots);
            LaunchRayCasts(rightChildPivots);
            index++;

            yield return new WaitForSeconds(1);

        } while (index < rotationTime);

        SetActive(false);
    }

    private void SetActive(bool b)
    {
        canRotate = b;
        if(b)
        {
            rightWater.Play();
            leftWater.Play();
        }
        else
        {
            rightWater.Stop();
            leftWater.Stop();
        }
    }

    private IEnumerator ActivateWater()
    {
        while (true)
        {
            if(TimeManager.instance.IsDay())
            {
                StartCoroutine(DoWater());
            }
            yield return new WaitForSeconds(time);
        }
    }

    private void LaunchRayCasts(Transform[] t)
    {
        for (int i = 0; i < t.Length; i++)
        {
            Ray ray = new Ray(t[i].position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.CompareTag("Hole"))
                {
                    hit.transform.GetComponent<HoleController>().AddWater(100.0f);
                }
            }
        }
    }
}
