using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class PlayerMovementSystem : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashSpeedChangeFactor;

    [SerializeField] public float maxYSpeed;
    [SerializeField] private float groundDrag;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private int doubleJumpCounter = 1;
    [HideInInspector] private bool readyToJump;

    [Header("Land")]
    [SerializeField] private float veryLowTimeLanding = 0.2f;
    [SerializeField] private float lowTimeLanding = 0.5f;
    [SerializeField] private float middleTimeLanding = 1.0f;
    [SerializeField] private float highTimeLanding = 2.0f;

    [Header("Inputs")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform orientation;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isGrounded;
    
    [Header("Feedback")]
    [SerializeField] private MMFeedbacks jumpFeedback;
    [SerializeField] private MMFeedbacks doubleJumpFeedback;
    [SerializeField] private MMFeedbacks landingFeedback;

    // Constants variables
    private const float lowVelocity = 0.1f;

    // Internal variables
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private EMoveState lastState;
    private bool keepMomentum;
    private float speedChangeFactor;
    private EMoveState state;
    private bool landing;
    private float velocityLastFrame;
    private float timeInAir;
    private int currentDoubleJumps;
    
    public enum EMoveState
    {
        walking,
        dashing,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        currentDoubleJumps = doubleJumpCounter;
    }

    // Update
    private void Update()
    {
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (state == EMoveState.walking)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    // Fixed update
    private void FixedUpdate()
    {
        MovePlayer();

        // landing
        // Get land point. Were going down last frame, and now reached an almost null velocity
        if (isGrounded && landing && (velocityLastFrame < 0) && (Mathf.Abs(rb.velocity.y) < lowVelocity))
        {   
            if (timeInAir >= highTimeLanding)
            {
                landingFeedback.PlayFeedbacks();
            }
            else if (timeInAir >= middleTimeLanding)
            {
                landingFeedback.PlayFeedbacks();
            }
            else if (timeInAir >= lowTimeLanding)
            {
            }
            else
            {
            }
                
            landing = false;
            timeInAir = 0;
        }
        velocityLastFrame = rb.velocity.y;
        
        // Count the time the player is landing
        if (landing)
        {
            timeInAir += Time.fixedDeltaTime;
        }
    }

    // Functions
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey))
        {
            // Jump on ground
            if (readyToJump && isGrounded)
            {
                readyToJump = false;

                Jump();
                jumpFeedback.PlayFeedbacks();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            // Double Jump in air
            else if (readyToJump && landing && currentDoubleJumps > 0)
            {
                Jump();
                doubleJumpFeedback.PlayFeedbacks();

                currentDoubleJumps--;
                timeInAir = 0;
            }
        }
    }

    private void MovePlayer()
    {
        if (state == EMoveState.dashing) return;
        
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (isGrounded)
        { 
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        // Esto a lo mejor tengo que caparlo a partir de la altura del jugador
        // limit y vel
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
    }
    
    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StateHandler()
    {
        // Mode - Dashing
        if (isDashing)
        {
            state = EMoveState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;

            timeInAir = 0;
        }
        
        // Mode - Walking
        else if (isGrounded)
        {
            currentDoubleJumps = doubleJumpCounter;
            state = EMoveState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            landing = true;
            state = EMoveState.air;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == EMoveState.dashing) keepMomentum = true;
        
        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }
}