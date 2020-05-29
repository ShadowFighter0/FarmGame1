using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
    public GameObject indicatorPrefab;
    private GameObject indicator;
    private GameObject holeManager;

    private void Start()
    {
        indicator = Instantiate(indicatorPrefab);
        holeManager = FindObjectOfType<HoleManager>().gameObject;
    }
    void Update()
    {
        GameObject go = RayCastController.instance.GetTarget();
        if(go != null)
        {
            if(go.CompareTag("Ground") || go.CompareTag("Hole"))
            {
                SetColor(go);
                indicator.SetActive(true);
            }
            else if(indicator.activeSelf)
            {
                indicator.SetActive(false);
            }
        }
        if (InputManager.instance.playerAnim != null)
        {
            if (Input.GetKeyDown(InputManager.instance.Click))
            {
                if (InputManager.state == InputManager.States.Idle && go.CompareTag("Ground"))
                {
                    InputManager.instance.playerAnim.SetTrigger("Dig");
                }
            }
        }
    }

    private void OnEnable()
    {
        if (indicator != null)
        {
            indicator.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
    }
    public void CreateHole()
    {
        Vector3 pos = indicator.transform.position;
        GameObject hole = ObjectPooler.Instance.SpawnFromPool("Holes", pos, Quaternion.identity);
        hole.transform.SetParent(holeManager.transform);
        hole.GetComponent<HoleController>().DoVFX();
    }
    private void SetColor(GameObject go)
    {
        MeshRenderer m = indicator.GetComponentInChildren<MeshRenderer>();
        if (go.CompareTag("Ground"))
        {
            Color c = new Color(102/255, 1, 20/255, 0.3f);
            m.material.color = c;
        }
        else
        {
            Color c = new Color(1, 20 / 255, 20 / 255, 0.3f);
            m.material.color = c;
        }
    }
}
