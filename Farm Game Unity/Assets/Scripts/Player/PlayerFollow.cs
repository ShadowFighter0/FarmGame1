using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;
    public Transform editTarget;
    
    private float inputSensitivity = 150.0f;

    Vector3 rotation;
    private const float clampAngle = 80.0f;

    private bool canMove = true;

    public float springK = 10.0f;
    public float damping = 5.0f;
    private Vector3 idealPos;
    private Vector3 cameraVel = new Vector3(0.0f, 0.0f, 0.0f);
    private float distance;
    public float maxDistance;

    public static PlayerFollow instance;
    private Vector3 target;

    public Vector3 shopRotation;
    private Transform cam;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotation = rot;

        if(player == null)
        {
            MovementController m = FindObjectOfType<MovementController>();
            player = m.transform;
        }
        cam = transform.GetChild(0);
        idealPos = player.position + cam.right * 0.3f + player.up * 1.1f;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        
        if(InputManager.state != InputManager.States.Editing)
        {
            if (InputManager.state == InputManager.States.Dialoguing)
            {
                DialogueCamPosition(dt);
            }
            else if (canMove)
            {
                Rotation(dt);
            }
            CameraUpdater(dt);
        }

        //inputSensitivity = inputSlider.value;
    }

    public void SetMouseSens(float value)
    {
        inputSensitivity = value;
    }

    private void DialogueCamPosition(float dt)
    {
        Quaternion localRotation = Quaternion.Euler(shopRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, localRotation, 10 * dt);
    }

    private void Rotation(float dt)
    {
        Quaternion localRotation;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float sensivility = GameManager.instance.MouseSensivility;
        rotation.y += mouseX * sensivility * dt;
        rotation.x -= mouseY * sensivility * dt;

        rotation.x = Mathf.Clamp(rotation.x, -clampAngle, clampAngle);
        localRotation = Quaternion.Euler(rotation);
        transform.rotation = localRotation;
    }

    void CameraUpdater(float dt)
    {
        Vector3 pos = SmoothFollow(dt);
        transform.position = pos;
    }

    public void SetRotation(Vector3 v)
    {
        shopRotation = v;
    }
    public void ChangeTarget(Vector3 t) { target = t; }

    private Vector3 SmoothFollow(float dt)
    {
        Vector3 pos = transform.position;

        distance = maxDistance;
        if (InputManager.state == InputManager.States.Dialoguing)
        {
            idealPos = target;
        }
        else if(MovementController.instance.IsMoving())
        {
            idealPos = player.position + cam.right * 0.3f + player.up * 1.1f;
        }

        Vector3 offset = idealPos - pos;
        float damp = Mathf.Min(1.0f, damping * dt);
        cameraVel *= 1.0f - damp;
        Vector3 springAccel = offset * springK;
        cameraVel += springAccel * dt;

        Vector3 movement = cameraVel * dt;
        pos += movement;

        offset = pos - idealPos;
        float sqrOffset = offset.sqrMagnitude;
        if (sqrOffset > distance * distance)
        {
            float offsetDis = Mathf.Sqrt(sqrOffset);
            offset *= (distance / offsetDis);
            pos = idealPos + offset;
        }
        return pos;
    }

    public void SetMovement(bool b) { canMove = b; }
}