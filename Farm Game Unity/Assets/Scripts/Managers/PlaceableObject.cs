using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Both are disabled
/// Canvas is disabled
/// 
/// child 0 -> bones
/// child 1 -> contruction
/// 
/// </summary>

public class PlaceableObject : MonoBehaviour
{
    bool menuActive = false;
    float speed = 50;

    [Tooltip("Head of player")] public GameObject origin;
    public GameObject menu;
    GameObject constructionId;

    void Update()
    {
        float dt = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Tab)) //move to script with all tools  action of tool
        {
            if(!menuActive)
            {
                menuActive = true;
                menu.SetActive(true);
            }
            else
            {
                menuActive = false;
                menu.SetActive(false);
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit) && constructionId != null)
        {
            if(hit.transform.gameObject.CompareTag("Ground"))
            {
                constructionId.transform.position = hit.point;
            }
        }

        if(Input.GetKey(KeyCode.E) || Input.mouseScrollDelta.y < 0)
        {
            constructionId.transform.Rotate(transform.up * speed * dt);
        }
        if(Input.GetKey(KeyCode.Q) || Input.mouseScrollDelta.y > 0)
        {
            constructionId.transform.Rotate(transform.up * -speed * dt);
        }
            
        if(Input.GetKey(KeyCode.Mouse0) && constructionId != null && !menuActive)
        {
            constructionId.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(constructionId.transform.GetChild(0).gameObject);
            
            constructionId = null;
        }
    }

    public void SetSelection(GameObject selection)
    { 
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit, 50f))
        {
            constructionId = Instantiate(selection);

            constructionId.transform.GetChild(0).gameObject.SetActive(true);

            constructionId.transform.position = hit.transform.position;
        }

    }
}
