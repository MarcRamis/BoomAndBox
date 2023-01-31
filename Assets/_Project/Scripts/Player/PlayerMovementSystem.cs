using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;
public enum EMoveState { WALKING, DASHING, AIMING, AIR }

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementSystem : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashSpeedChangeFactor;
    [SerializeField] public float maxYSpeed;
    [SerializeField] private float groundDrag;
    [HideInInspector] private float horizontalInput;
    [HideInInspector] private float verticalInput;
    [HideInInspector] private Vector3 moveDirection;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isAiming;

    [Header("Orientation")]
    [SerializeField] private float modelRotationSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private int doubleJumpCounter = 1;
    [HideInInspector] private bool readyToJump;
    [SerializeField] private float coyoteTime = 0.2f;
    private int currentDoubleJumps;
    private bool isDoubleJumping;
    private float coyoteTimeCounter;

    [Header("Land")]
    [SerializeField] private float veryLowTimeLanding = 0.2f;
    [SerializeField] private float lowTimeLanding = 0.5f;
    [SerializeField] private float middleTimeLanding = 1.0f;
    [SerializeField] private float highTimeLanding = 2.0f;
    private float timeInAir;
    private bool landing;
    private float velocityLastFrame;

    //Inputs
    [HideInInspector] public Vector2 _look;
    [HideInInspector] public PlayerInputController myInputs;

    [Header("Ground Check")]
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform groundTransform;
    [HideInInspector] public bool isGrounded;

    [Header("References")]
    [SerializeField] private Transform model;
    [SerializeField] private Camera mainCamera;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks jumpFeedback;
    [SerializeField] private MMFeedbacks doubleJumpFeedback;
    [SerializeField] private MMFeedbacks landingFeedback;

    // Constants variables
    private const float lowVelocity = 0.1f;

    // Internal variables
    private Rigidbody m_Rb;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private bool keepMomentum;
    private float speedChangeFactor;
    private EMoveState lastState;
    
    [HideInInspector] public EMoveState movementState;

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        // Initialize inputs
        myInputs.OnJumpPerformed += DoJump;
        myInputs.OnZoomPerformed += DoZoom;

        // Initalize properties
        m_Rb.freezeRotation = true;
        readyToJump = true;
        currentDoubleJumps = doubleJumpCounter;
    }

    // Update
    private void Update()
    {
        // Ground check
        CheckGround();
        
        MyInput();
        SpeedControl();
        HandleMovementState();

        // Handle drag
        HandleDrag();
    }

    private void CheckGround()
    {
        var hitColliders = Physics.OverlapSphere(groundTransform.position, groundRadius, whatIsGround);
        isGrounded = hitColliders.Length > 0;
    }

    // Fixed update
    private void FixedUpdate()
    {
        MovePlayer();
        OnLand();
        // Rotate player
        RotateModel();
    }

    // Functions
    private void HandleDrag()
    {
        if (movementState == EMoveState.WALKING || (movementState == EMoveState.AIMING && isGrounded) || isDoubleJumping)
            m_Rb.drag = groundDrag;
        else
            m_Rb.drag = 0;
    }
    private void MyInput()
    {
        // Take input directions
        horizontalInput = myInputs.moveDirection.x;
        verticalInput = myInputs.moveDirection.y;



        // Coyote time
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
    }

    private void DoZoom()
    {
        isAiming = !isAiming;
    }

    private void DoJump()
    {
        // Jump on ground
        /// <<summary>
        /// Only can jump when is grounded or in coyote time
        /// </summary>
        if ((readyToJump && isGrounded) || (readyToJump && coyoteTimeCounter > 0f))
        {
            coyoteTimeCounter = 0f;
            readyToJump = false; 

            ApplyJumpForce();
            jumpFeedback.PlayFeedbacks();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Double Jump in air
        /// <summary>
        /// Only can jump when jump cooldown is ready to prevent the double jump spam
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

            ApplyJumpForce();
            doubleJumpFeedback.PlayFeedbacks();

            currentDoubleJumps--;
            timeInAir = 0;

            Invoke(nameof(ResetDoubleJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (movementState == EMoveState.DASHING) return;
        
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (isGrounded)
        {
            m_Rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        
        // In air
        else
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
    
    private void ApplyJumpForce()
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

    private void HandleMovementState()
    {
        // Mode - Dashing
        if (isDashing)
        {
            movementState = EMoveState.DASHING;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;

            timeInAir = 0;
        }

        // Mode - Aiming
        //else if (isAiming)
        //{
        //    movementState = EMoveState.AIMING;
        //    desiredMoveSpeed = aimSpeed;
        //}
        
        // Mode - Walking
        else if (isGrounded)
        {
            currentDoubleJumps = doubleJumpCounter;
            movementState = EMoveState.WALKING;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            landing = true;
            movementState = EMoveState.AIR;
        }
        
        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == EMoveState.DASHING) keepMomentum = true;
        
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
        lastState = movementState;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundTransform.position, groundRadius);
    }

    private void RotateModel()
    {
        // rotate orientation
        Vector3 viewDir = transform.position - new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            model.forward = Vector3.Slerp(model.forward, inputDir.normalized, Time.fixedDeltaTime * modelRotationSpeed);
    }
}