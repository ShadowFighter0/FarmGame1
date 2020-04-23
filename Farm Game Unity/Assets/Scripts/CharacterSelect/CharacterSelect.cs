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
    public RuntimeAnimatorController animContr;

    public GameObject def;
    public Transform player;
    public Text customizeText;

    private float pov;
    private float oriPov;

    private float oriY;

    private Vector3 selectPos;
    private bool deleting;

    public Transform maleTools;
    public Transform femaleTools;

    void Start()
    {
        selected = def;
        pov = Camera.main.fieldOfView;
        oriPov = pov;
        customizeFolder.SetActive(false);
        changeChrFolder.SetActive(true);
        custButton.SetActive(false);

        oriY = selected.transform.localEulerAngles.y;
        selectPos = selected.transform.position;
    }

    void Update()
    {
        if(!deleting)
        {
            float dt = Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, pov, dt * 10f);

            Vector3 pos = selectPos;
            pos.y = transform.position.y;
            pos.z = transform.position.z;

            transform.position = Vector3.Lerp(transform.position, pos, dt * 10f);

            if (Input.GetMouseButton(0))
            {
                float mouseX = -Input.GetAxis("Mouse X");
                selected.transform.localEulerAngles += Vector3.up * mouseX * dt * 360f;
            }
            else
            {
                selected.transform.localEulerAngles = Vector3.Lerp(selected.transform.localEulerAngles, Vector3.up * oriY, dt * 10f);
            }
        }
    }
    public void SetTarget(GameObject t)
    {
        custButton.SetActive(true);
        if (selected != t)
        {
            selected = t;
            selectPos = selected.transform.position;
            oriY = selected.transform.eulerAngles.y;
            selected.GetComponent<ChangeHat>().Selected();
        }
    }

    public void Finish()
    {
        ChangeHat script = selected.GetComponent<ChangeHat>();
        script.Finish();
        customizeFolder.SetActive(false);
        changeChrFolder.SetActive(false);
        custButton.SetActive(false);

        GameManager.instance.SetPlayerCustomization(script.GetIndex(), selected.transform.GetSiblingIndex(), script.GetGlasses());
        GameManager.instance.SavePlayer();

        Destroy(selected.GetComponent<ChangeHat>());
        if (selected.CompareTag("Male"))
        {
            GameObject go = GameObject.FindGameObjectWithTag("Female");
            Destroy(go.GetComponent<ChangeHat>());
            go.SetActive(false);
        }
        else
        {
            GameObject go = GameObject.FindGameObjectWithTag("Male");
            Destroy(go.GetComponent<ChangeHat>());
            go.SetActive(false);
        }

        
        selected.transform.SetParent(null);
        player.position = selected.transform.position;
        player.rotation = selected.transform.rotation;

        deleting = true;
        StartCoroutine(ActiveMovement());
    }
    IEnumerator ActiveMovement()
    {
        yield return new WaitForSeconds(0.2f);
        selected.transform.SetParent(player);

        MovementController.instance.enabled = true;
        MovementController.instance.SetAnimator(selected.GetComponent<Animator>());

        PlayerFollow.instance.enabled = true;
        if(selected.name.Equals("Male"))
        {
            InputManager.instance.SetTools(maleTools);
        }
        else
        {
            InputManager.instance.SetTools(femaleTools);
        }
        
        InputManager.instance.enabled = true;

        Animator selAnim = selected.GetComponent<Animator>();
        selAnim.runtimeAnimatorController = animContr;

        InputManager.instance.ChangeState(InputManager.States.Idle);
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
