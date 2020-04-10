using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoeController : MonoBehaviour
{
    public GameObject indicator;
    [SerializeField] GameObject hole = null;
    private GameObject holeManager;
    private void Start()
    {
        holeManager = FindObjectOfType<HoleManager>().gameObject;
    }
    void Update()
    {
        if(InputManager.state == InputManager.States.Working)
        {
            GameObject go = RayCastController.instance.GetTarget();
            if(go != null)
            {
                SetColor(go);
            }

            if (Input.GetMouseButton(0))
            {
                Dig(go);
            }
        }
    }

    private void Dig(GameObject go)
    {
        if (go.CompareTag("Ground"))
        {
            Instantiate(hole, indicator.transform.position, Quaternion.identity).transform.SetParent(holeManager.transform);
        }
    }

    private void SetColor(GameObject go)
    {
        MeshRenderer m = indicator.GetComponent<MeshRenderer>();
        if (go.CompareTag("Ground"))
        {
            Color c = new Color(102/255, 1, 20/255, 0.7f);
            m.material.color = c;
        }
        else
        {
            Color c = new Color(1, 20 / 255, 20 / 255, 0.7f);
            m.material.color = c;
        }
    }
}
