using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTrees : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = Input.mousePosition;
        vec.z = 1;
        ray = Camera.main.ScreenPointToRay(vec);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.gameObject.CompareTag("Tree"))
                {
                    hit.transform.gameObject.GetComponent<TreeLife>().TDamage();
                }

            }
        }
           
    }
}
