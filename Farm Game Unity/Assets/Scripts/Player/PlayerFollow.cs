using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;
    public Transform editTarget;
    
    private const float inputSensitivity = 150.0f;

    Vector3 rotation;
    private const float clampAngle = 80.0f;

    private bool canMove = true;

    public const float springK = 10.0f;
    public const float damping = 5.0f;
    private Vector3 idealPos;
    private Vector3 cameraVel = new Vector3(0.0f, 0.0f, 0.0f);
    private float distance;
    public float maxDistance;

    public static PlayerFollow instance;
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
        idealPos = player.position;

        SetMovement(true);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if(canMove)
        {
            Rotation(dt);
        }

        CameraUpdater(dt);
    }

    private void Rotation(float dt)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotation.y += mouseX * inputSensitivity * dt;
        rotation.x -= mouseY * inputSensitivity * dt;

        rotation.x = Mathf.Clamp(rotation.x, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotation);
        transform.rotation = localRotation;
    }

    void CameraUpdater(float dt)
    {
        Vector3 pos = SmoothFollow(dt);
        transform.position = pos;
    }

    private Vector3 SmoothFollow(float dt)
    {
        Vector3 pos = transform.position;
        if (InputManager.instance.editing)
        {
            idealPos = editTarget.position;
            distance = 0;
        }
        else
        {
            distance = maxDistance;
            if(rotation.sqrMagnitude > Mathf.Epsilon)
            {
                idealPos = player.position + player.right * 0.3f + player.up * 1.1f;
            }
            else
            {
                idealPos = player.position + transform.right * 0.3f + transform.up * 1.1f;
            }
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

    public void SetMovement(bool b)
    {
        canMove = b;
        if (!canMove)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}