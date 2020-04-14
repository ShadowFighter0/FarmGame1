using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkShopCameraController : MonoBehaviour
{
    public Transform cam;
    public Transform camPivot;
    private Vector3 newPos;
    private Vector3 newPivotPos;
    public float speed;
    private RaycastHit hit;
    private Ray ray;

    public GameObject unlockableItemMenu;
    private void Start()
    {
        camPivot = transform.GetChild(0);
        newPos = transform.position;
        newPivotPos = camPivot.localPosition;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        Movement(dt);

        if(Input.GetKeyDown(InputManager.instance.Click))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.CompareTag("UnlockableItem"))
                {
                    unlockableItemMenu.SetActive(true);
                    newPos = hit.transform.position;
                    //show menu
                }
            }
        }
    }

    private void Movement(float dt)
    {
        Vector3 input;
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");
        input.y = 0;

        int scroll = (int)Input.mouseScrollDelta.y;

        if (input.sqrMagnitude > Mathf.Epsilon)
        {
            newPos += input * speed * dt;
        }

        if(scroll != 0)
        {
            newPivotPos.y -= scroll;
            //newPivotPos.y = Mathf.Clamp(newPos.y, 5f, 15f);
        }

        cam.position = camPivot.position;
        cam.rotation = camPivot.rotation;

        transform.position = Vector3.Lerp(transform.position, newPos, 10 * dt);
        camPivot.localPosition = Vector3.Lerp(camPivot.localPosition, newPivotPos, 10 * dt);
    }
}
