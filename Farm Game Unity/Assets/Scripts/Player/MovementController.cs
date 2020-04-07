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
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float accel = 10f;
    private float maxSpeed;
    private float currentSpeed;

    [SerializeField] private float gravityAmount = 9.8f;
    private float gravity = 0;
    private bool jumping = true;

    public bool canMove = true;
    public Transform cam;

    private Animator anim;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    private void Start()
    {
        maxSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        float dt = Time.deltaTime;
        Move(dt);
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
        else
        {
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
        }
        
        if (InputManager.instance.editing)
        {
            EditMovement(dt, moveInputs);
        }
        else
        {
            NormalMovement(dt, moveInputs);
        }
    }

    private void NormalMovement(float dt, Vector2 moveInputs)
    {
        if (moveInputs.sqrMagnitude > Mathf.Epsilon)
        {
            anim.SetBool("walking", true);
            if (!jumping)
            {
                Rotation(moveInputs);
            }
        }
        else
        {
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
        }

        ChangeSpeed();
        Jump(dt);

        currentSpeed += GetOffset(dt, moveInputs.magnitude, maxSpeed, currentSpeed, accel);

        Vector3 newPos = transform.forward * currentSpeed + transform.up * gravity;
        controller.Move(newPos * dt);
    }

    private void EditMovement(float dt, Vector2 moveInputs)
    {
        if (moveInputs.sqrMagnitude > Mathf.Epsilon)
        {
            anim.SetBool("walking", true);
            if (!jumping)
            {
                Rotation(moveInputs);
            }
        }
        else
        {
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
        }

        maxSpeed = walkSpeed;
        anim.SetBool("running", false);

        offset.x += GetOffset(dt, moveInputs.x, maxSpeed, offset.x, accel);
        offset.z += GetOffset(dt, moveInputs.y, maxSpeed, offset.z, accel);

        Vector3 newPos = (transform.forward * offset.z + transform.right * offset.x);
        controller.Move(newPos * dt);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Jump(float dt)
    {
        gravity -= gravityAmount * dt;
        if (Input.GetKey(KeyCode.Space) && !jumping)
        {
            jumping = true;
            gravity = 5f;
        }

        if (controller.isGrounded)
        {
            jumping = false;
            gravity = 0;
        }
    }

    private void ChangeSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("running", true);
            maxSpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("running", false);
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
