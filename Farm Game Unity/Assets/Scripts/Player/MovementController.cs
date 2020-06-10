using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Vector3 offset;

    public static MovementController instance;
    private void Awake()
    {
        instance = this;
    }

    private CharacterController controller;
    public float runSpeed = 2f;
    public float walkSpeed = 1f;
    public float accel = 10f;
    private float maxSpeed;
    private float currentSpeed;

    public float gravityAmount = 7f;
    private float gravity = 0;
    private bool jumping = true;

    public bool canMove = true;
    public Transform cam;

    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    private void Start()
    {
        maxSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        jumping = false;
        gravity = 0;
    }
    void Update()
    {
        if(InputManager.instance.playerAnim != null)
        {
            float dt = Time.deltaTime;
            Move(dt);
        }
    }
    public void StopMovement()
    {
        currentSpeed = 0;
    }
    private void Move(float dt)
    {
        Vector2 moveInputs;
        moveInputs = Vector2.zero;
        if (canMove)
        {
            moveInputs.x = Input.GetAxisRaw("Horizontal");
            moveInputs.y = Input.GetAxisRaw("Vertical");
        }
        NormalMovement(dt, moveInputs);
    }

    private void NormalMovement(float dt, Vector2 moveInputs)
    {
        if (moveInputs.sqrMagnitude > Mathf.Epsilon)
        {
            if (!jumping)
            {
                Rotation(moveInputs);
            }
        }

        ChangeSpeed();
        Jump(dt);

        currentSpeed += GetOffset(dt, moveInputs.magnitude, maxSpeed, currentSpeed, accel);
        InputManager.instance.playerAnim.SetFloat("Vel", currentSpeed);

        Vector3 newPos = (transform.forward * currentSpeed + transform.up * gravity);
        controller.Move(newPos * dt);
    }

    public bool IsMoving() { return currentSpeed > Mathf.Epsilon;}
    public void SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;
    }
    private void EditMovement(float dt, Vector2 moveInputs)
    {
        if (moveInputs.sqrMagnitude > Mathf.Epsilon)
        {
            if (!jumping)
            {
                Rotation(moveInputs);
            }
        }

        maxSpeed = walkSpeed;

        offset.x += GetOffset(dt, moveInputs.x, maxSpeed, offset.x, accel);
        offset.z += GetOffset(dt, moveInputs.y, maxSpeed, offset.z, accel);

        Vector3 newPos = (transform.forward * offset.z + transform.right * offset.x).normalized;
        controller.Move(newPos * dt);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Jump(float dt)
    {
        gravity -= gravityAmount * dt;
        
        if (controller.isGrounded)
        {
            jumping = false;
            gravity = 0;
        }
    }

    private void ChangeSpeed()
    {
        if (Input.GetKeyDown(InputManager.instance.Run))
        {
            maxSpeed = runSpeed;
        }
        if (Input.GetKeyUp(InputManager.instance.Run))
        {
            maxSpeed = walkSpeed;
        }
    }

    private void Rotation(Vector2 moveInputs)
    {
        float targetRotation = Mathf.Atan2(moveInputs.x, moveInputs.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
    }

    private float GetOffset(float dt, float input, float max, float currentSpeed, float accel)
    {
        float targetZSpeed = input * max;
        float velZOffset = targetZSpeed - currentSpeed;
        velZOffset = Mathf.Clamp(velZOffset, -accel * dt, accel * dt);
        return velZOffset;
    }
    public void SetMovement(bool b) { canMove = b; }
}
