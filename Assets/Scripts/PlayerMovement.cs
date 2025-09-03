using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;

    private InputActionAsset inputAsset;
    public InputActionMap player;
    private InputAction move;
    private InputAction jump;
    private InputAction crouch;

    // Movement
    public float moveSpeed = 10f;
    public float airMultiplier = 0.5f;
    public float groundDrag = 4f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float jumpCooldown = 0.25f;
    private bool readyToJump = true;

    [Header("Crouching / Sliding")]
    public float crouchSpeed = 5f;
    public float slideForce = 5f;
    public Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public bool grounded;

    private Vector2 moveInput;
    private bool jumpPressed;
    private bool crouchHeld;
    private bool crouchingState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerScale = transform.localScale;

        // Get Input Actions
        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");

        move = player.FindAction("Move");
        jump = player.FindAction("Jump");
        crouch = player.FindAction("Crouch");
    }

    private void OnEnable()
    {
        jump.performed += ctx => jumpPressed = true;
        crouch.performed += ctx => crouchHeld = true;
        crouch.canceled += ctx => crouchHeld = false;

        player.Enable();
    }

    private void OnDisable()
    {
        jump.performed -= ctx => jumpPressed = true;
        crouch.performed -= ctx => crouchHeld = true;
        crouch.canceled -= ctx => crouchHeld = false;

        player.Disable();
    }

    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        HandleCrouch();

        // Drag
        rb.linearDamping = grounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveInput = move.ReadValue<Vector2>();

        Vector3 moveDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        moveDir.Normalize();

        if (grounded)
            rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        if (jumpPressed && readyToJump && grounded)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void Jump()
    {
        readyToJump = false;

        // Reset Y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleCrouch()
    {
        if (crouchHeld && !crouchingState)
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            crouchingState = true;

            if (rb.linearVelocity.magnitude > 0.5f && grounded)
                rb.AddForce(orientation.forward * slideForce, ForceMode.Impulse);
        }
        else if (!crouchHeld && crouchingState)
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            crouchingState = false;
        }
    }
}
