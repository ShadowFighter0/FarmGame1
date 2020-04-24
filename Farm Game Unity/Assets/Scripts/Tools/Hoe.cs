using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
    public GameObject indicator;
    private GameObject holeManager;
    private Animator anim;
    private void Awake()
    {
        GameEvents.OnAnimatorSelected += SetAnimator;
    }
    private void Start()
    {
        holeManager = FindObjectOfType<HoleManager>().gameObject;
        
    }
    void Update()
    {
        GameObject go = RayCastController.instance.GetTarget();
        if(go != null)
        {
            SetColor(go);
        }

        if (Input.GetKeyDown(InputManager.instance.Click))
        {
            Dig(go);
        }
    }

    public void SetAnimator(Animator an)
    {
        anim = an;
    }

    private void OnEnable()
    {
        indicator.SetActive(true);
    }
    private void OnDisable()
    {
        indicator.SetActive(false);
    }
    private void Dig(GameObject go)
    {
        if (go.CompareTag("Ground"))
        {
            Vector3 pos = indicator.transform.position;
            GameObject hole = ObjectPooler.Instance.SpawnFromPool("Holes", pos, Quaternion.identity);
            if(anim != null)
            {
                anim.SetTrigger("Dig");
            }
            
            hole.transform.SetParent(holeManager.transform);
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
