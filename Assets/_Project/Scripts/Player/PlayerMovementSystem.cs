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
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Land")]
    [SerializeField] private float veryLowTimeLanding = 0.2f;
    [SerializeField] private float lowTimeLanding = 0.5f;
    [SerializeField] private float middleTimeLanding = 1.0f;
    [SerializeField] private float highTimeLanding = 2.0f;
    private bool landing;
    private float velocityLastFrame;

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
    private Rigidbody m_Rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private EMoveState lastState;
    private bool keepMomentum;
    private float speedChangeFactor;
    private EMoveState state;
    private float timeInAir;
    private int currentDoubleJumps;
    private bool isDoubleJumping;
    
    public enum EMoveState
    {
        walking,
        dashing,
        air
    }

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Rb.freezeRotation = true;

        readyToJump = true;
        currentDoubleJumps = doubleJumpCounter;
    }

    // Update
    private void Update()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // Handle drag
        HandleDrag();
    }

    // Fixed update
    private void FixedUpdate()
    {
        MovePlayer();
        OnLand();
    }

    // Functions
    private void HandleDrag()
    {
        if (state == EMoveState.walking || isDoubleJumping)
            m_Rb.drag = groundDrag;
        else
            m_Rb.drag = 0;
    }
    private void MyInput()
    {
        // Take input directions
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // coyote time
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // when to jump
        if (Input.GetKeyDown(jumpKey))
        {
            // Jump on ground
            /// <<summary>
            /// Only can jump when is grounded or in coyote time
            /// </summary>
            if ((readyToJump && isGrounded) || (readyToJump && coyoteTimeCounter > 0f))
            {
                coyoteTimeCounter = 0f;
                readyToJump = false;

                Jump();
                jumpFeedback.PlayFeedbacks();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            // Double Jump in air
            /// <summary>
            /// Only can jump when is jump cooldown is ready to prevent the double jump spam
            /// 
            /// There is a counter of double jumps in air the player can make to change if it's necessary
            /// 
            /// Coyote time is applied but not really necessary. Only to prevent the player doesn't double jump when in coyoteTime 
            /// because it mustn't count
            /// 
            /// </summary> 
            else if (readyToJump && landing && currentDoubleJumps > 0 && !isDashing && coyoteTimeCounter <= 0f)
            {
                isDoubleJumping = true;
                
                Jump();
                doubleJumpFeedback.PlayFeedbacks();

                currentDoubleJumps--;
                timeInAir = 0;

                Invoke(nameof(ResetDoubleJump), jumpCooldown);
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
            m_Rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!isGrounded)
        {
            m_Rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    private void OnLand()
    {
        // Landing
        // Get land point. Were going down last frame, and now reached an almost null velocity
        if (isGrounded && landing && (velocityLastFrame < 0) && (Mathf.Abs(m_Rb.velocity.y) < lowVelocity))
        {
            // Different operations for different fall length landing 
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
            else if (timeInAir >= veryLowTimeLanding)
            {
            }

            // Reset landing
            landing = false;
            timeInAir = 0;
        }
        velocityLastFrame = m_Rb.velocity.y;

        // Count the time the player is landing
        if (landing)
        {
            timeInAir += Time.fixedDeltaTime;
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(m_Rb.velocity.x, 0f, m_Rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            m_Rb.velocity = new Vector3(limitedVel.x, m_Rb.velocity.y, limitedVel.z);
        }

        // Esto a lo mejor tengo que caparlo a partir de la altura del jugador
        // limit y vel
        if (maxYSpeed != 0 && m_Rb.velocity.y > maxYSpeed)
        {
            m_Rb.velocity = new Vector3(m_Rb.velocity.x, maxYSpeed, m_Rb.velocity.z);
        }
    }
    
    private void Jump()
    {
        // reset y velocity
        m_Rb.velocity = new Vector3(m_Rb.velocity.x, 0f, m_Rb.velocity.z);

        m_Rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    private void ResetDoubleJump()
    {
        isDoubleJumping = false;
    }

    private void StateHandler()
    {
        // Mode - Dashing
        if (isDashing)
        {
            state = EMoveState.dashing;
            //desiredMoveSpeed = dashSpeed;
            //speedChangeFactor = dashSpeedChangeFactor;

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