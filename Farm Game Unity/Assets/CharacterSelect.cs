using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    private GameObject selected = null;

    public GameObject changeChrFolder;
    public GameObject customizeFolder;
    public GameObject custButton;

    public Text customizeText;
    private float pov;
    private float oriPov;

    private float oriY;

    void Start()
    {
        pov = Camera.main.fieldOfView;
        oriPov = pov;
        customizeFolder.SetActive(false);
        changeChrFolder.SetActive(true);
        custButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if(selected != null)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, pov, dt * 10f);

            Vector3 dir = selected.transform.position - transform.position;
            dir.y = 0.0f;

            Quaternion rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, rot, dt * 10f);

            if (Input.GetMouseButton(0))
            {
                float mouseX = -Input.GetAxis("Mouse X");
                selected.transform.eulerAngles += Vector3.up * mouseX * dt * 360f;
            }
            else
            {
                selected.transform.eulerAngles = Vector3.Lerp(selected.transform.eulerAngles, Vector3.up * oriY, dt * 10f);
            }
        }
    }
    public void SetTarget(GameObject t)
    {
        custButton.SetActive(true);
        if (selected != t)
        {
            selected = t;
            oriY = selected.transform.eulerAngles.y;
            selected.GetComponent<ChangeHat>().Selected();
        }
    }

    public void Finish()
    {
        customizeFolder.SetActive(false);
        changeChrFolder.SetActive(false);
        custButton.SetActive(false);

        Destroy(selected.GetComponent<ChangeHat>());

        if(selected.CompareTag("Male"))
        {
            GameObject go = GameObject.FindGameObjectWithTag("Female");
            Destroy(go);
        }
        else
        {
            GameObject go = GameObject.FindGameObjectWithTag("Male");
            Destroy(go);
        }

        GameEvents.PlayerSelected(selected.transform);
        Destroy(this);
    }
    public void Customize()
    {
        customizeFolder.SetActive(!customizeFolder.activeSelf);
        if(customizeFolder.activeSelf)
        {
            pov = 30;
            customizeText.text = "DONE";
            changeChrFolder.SetActive(false);
            customizeFolder.SetActive(true);
        }
        else
        {
            pov = oriPov;
            customizeText.text = "CUSTOMIZE";
            changeChrFolder.SetActive(true);
            customizeFolder.SetActive(false);
        }
    }
    public void ChangeSelectedHat(int dir)
    {
        selected.GetComponent<ChangeHat>().NewHat(dir);
    }
    public void ChangeGlasses()
    {
        selected.GetComponent<ChangeHat>().ChangeGlasses();
    }
}
