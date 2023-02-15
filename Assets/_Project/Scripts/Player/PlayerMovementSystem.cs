using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;
public enum EMoveState { WALKING, DASHING, AIMING, SLIDING, AIR }
public enum EAnimState { IDLE, RUNNING }

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform model;
    [SerializeField] private Camera mainCamera;
    [HideInInspector] private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider;
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [HideInInspector] private float moveSpeed;
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
    [HideInInspector] public bool justHitGround;
    
    [Header("Stairs")]
    [SerializeField] private Transform stepOffsetHigher;
    [SerializeField] private Transform stepOffsetLower;
    [SerializeField] private float stepOffsetRayLengthUpper = 0.2f;
    [SerializeField] private float stepOffsetRayLengthLower = 0.1f;
    [SerializeField] private float stepSmooth = 0.1f;

    [Header("Orientation")]
    [SerializeField] private Transform orientation;
    [SerializeField] public Transform fullOrientation;
    [SerializeField] private float modelRotationSpeed;
    [SerializeField] private Transform lookAt;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private int doubleJumpCounter = 1;
    [HideInInspector] public bool readyToJump;
    [SerializeField] private float coyoteTime = 0.2f;
    [HideInInspector] public int currentDoubleJumps;
    [HideInInspector] public bool isDoubleJumping;
    [HideInInspector] private float coyoteTimeCounter;

    [Header("Land")]
    [SerializeField] private float veryLowTimeLanding = 0.2f;
    [SerializeField] private float lowTimeLanding = 0.5f;
    [SerializeField] private float middleTimeLanding = 1.0f;
    [SerializeField] private float highTimeLanding = 2.0f;
    [HideInInspector] private float timeInAir;
    // Using a variable to know when it pressed the input jump because i want to know if is falling or not
    [HideInInspector] public bool isFalling;
    [HideInInspector] public bool landing;
    [HideInInspector] private float velocityLastFrame;

    //Inputs
    [HideInInspector] public Vector2 _look;
    [HideInInspector] public PlayerInputController myInputs;

    [Header("Ground Check")]
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundTransform;
    [HideInInspector] public bool isGrounded;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks jumpFeedback;
    [SerializeField] private MMFeedbacks doubleJumpFeedback;
    [SerializeField] private MMFeedbacks landingFeedback;
    [SerializeField] private MMFeedbacks landingFeedbackShort;
    [SerializeField] public TrailRenderer trailLeftShoe;
    [SerializeField] public TrailRenderer trailRightShoe;
    
    // Constants variables
    [HideInInspector] private const float lowVelocity = 0.1f;

    // Internal needed variables
    [HideInInspector] private float desiredMoveSpeed;
    [HideInInspector] private float lastDesiredMoveSpeed;
    [HideInInspector] private bool keepMomentum;
    [HideInInspector] private float speedChangeFactor;

    [HideInInspector] private float lastFramePosition;
    
    [HideInInspector] private EMoveState lastState;
    [HideInInspector] public EMoveState movementState;
    [HideInInspector] public EAnimState animState;
    
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
        
        TrailJumpFeedbackReset();
    }

    private void Start()
    {
        // Initialize inputs
        myInputs.OnJumpPerformed += DoJump;
        myInputs.OnZoomPerformed += DoZoom;

        // Initalize properties
        playerRigidbody.freezeRotation = true;
        readyToJump = true;
        currentDoubleJumps = doubleJumpCounter;

        // this is setted here to stepoffset 
        //stepOffsetHigher.transform.position = new Vector3(stepOffsetHigher.transform.position.x, stepHeight, stepOffsetHigher.transform.position.z);
    }

    // Update

    private void Update()
    {
        CheckGround();
        
        MyInput();
        SpeedControl();
        HandleMovementState();
        HandleAnimState();

        // Handle drag
        HandleDrag();
    }

    // Fixed update

    private void FixedUpdate()
    {
        CheckFalling();

        MovePlayer();
        RotateModel();
        //StepOffset();
        
        OnLand();
    }

    // Functions
    private void HandleDrag()
    {
        if (movementState == EMoveState.WALKING || (movementState == EMoveState.AIMING && isGrounded) || isDoubleJumping)
            playerRigidbody.drag = groundDrag;
        else
            playerRigidbody.drag = 0;
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
            TrailJumpFeedback();

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
            TrailJumpFeedback();

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
            playerRigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        
        // In air
        else
        {
            playerRigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    private void OnLand()
    {
        if (isGrounded)
        {
            justHitGround = false;
        }

        // Landing
        // Get land point. Were going down last frame, and now reached an almost null velocity
        if (isGrounded && landing && (velocityLastFrame < 0) && (Mathf.Abs(playerRigidbody.velocity.y) < lowVelocity))
        {
            // Different operations for different fall length landing 
            if (timeInAir >= highTimeLanding)
            {
                landingFeedback.PlayFeedbacks();
            }
            else if (timeInAir >= middleTimeLanding)
            {
                landingFeedbackShort.PlayFeedbacks();
            }
            else if (timeInAir >= lowTimeLanding)
            {
                landingFeedbackShort.PlayFeedbacks();
            }
            else if (timeInAir >= veryLowTimeLanding)
            {
                landingFeedbackShort.PlayFeedbacks();
            }
            
            justHitGround = true;

            // Reset landing
            landing = false;
            timeInAir = 0;
        }

        velocityLastFrame = playerRigidbody.velocity.y;

        // Count the time the player is landing
        if (landing)
        {
            //justHitGround = false;

            timeInAir += Time.fixedDeltaTime;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerRigidbody.velocity = new Vector3(limitedVel.x, playerRigidbody.velocity.y, limitedVel.z);
        }

        // Esto a lo mejor tengo que caparlo a partir de la altura del jugador
        // limit y vel
        if (maxYSpeed != 0 && playerRigidbody.velocity.y > maxYSpeed)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, maxYSpeed, playerRigidbody.velocity.z);
        }
    }
    
    private void ApplyJumpForce()
    {
        // reset y velocity
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
        else if (isAiming)
        {
            movementState = EMoveState.AIMING;
            desiredMoveSpeed = aimSpeed;
        }
        
        // Mode - Walking
        else if (isGrounded)
        {
            if (playerRigidbody.velocity.magnitude > 0.1f)
            {
                animState = EAnimState.RUNNING;
            }
            else
            {
                animState = EAnimState.IDLE;
            }

            movementState = EMoveState.WALKING;
            currentDoubleJumps = doubleJumpCounter;
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
    
    private void HandleAnimState()
    {
        switch (animState)
        {
            case EAnimState.IDLE:
                //playerCollider.radius = 0.56f;
                break;
            case EAnimState.RUNNING:
                //playerCollider.radius = 1.15f;
                break;
        }
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
    
    private void RotateModel()
    {
        // Transform with full orientation
        Vector3 viewDirFullOrientation = transform.position - mainCamera.transform.position;
        fullOrientation.forward = viewDirFullOrientation.normalized;
        
        // Transform only with orientation on x, z. Needed to just rotate the player in the input direction
        // but i use it to move the player to the camera direction

        if (!isAiming)
        {
            Vector3 viewDir = transform.position - new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
            orientation.forward = viewDir.normalized;
            
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                model.forward = Vector3.Slerp(model.forward, inputDir.normalized, Time.fixedDeltaTime * modelRotationSpeed);

                // searching the normal because i want to make the model can rotate on slope surfaces
                RaycastHit hit;
                if (Physics.Raycast(groundTransform.position, groundTransform.TransformDirection(-Vector3.up), out hit, 1.0f))
                {
                    Vector3 surfaceNormal = hit.normal;

                    if (!isGrounded) surfaceNormal = Vector3.up;

                    Quaternion targetRotation = Quaternion.FromToRotation(model.up, surfaceNormal) * model.rotation;
                    model.rotation = Quaternion.Slerp(model.rotation, targetRotation, modelRotationSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            Vector3 dirToCombatLookAt = lookAt.position - new Vector3(mainCamera.transform.position.x, lookAt.position.y, mainCamera.transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            model.forward = dirToCombatLookAt.normalized;
        }
    }

    private void StepOffset()
    {
        if (isGrounded)
        {
            // Doing this for only going on upstairws that arent sloping
            RaycastHit hitGroundNormal;
            //Vector3 offsetPosition = groundTransform.transform + transform.position;

            if (Physics.Raycast(groundTransform.position + (Vector3.forward * 0.5f), -Vector3.up, out hitGroundNormal, groundRadius))
            {
                if (hitGroundNormal.normal == Vector3.up)
                {
                    CheckStairsDirection(Vector3.forward);
                    CheckStairsDirection(new Vector3(1.5f, 0, 1));
                    CheckStairsDirection(new Vector3(-1.5f, 0, 1));
                }
            }

            
        }

    }
    private void CheckStairsDirection(Vector3 direction)
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepOffsetLower.position, stepOffsetLower.TransformDirection(direction), out hitLower, stepOffsetRayLengthLower))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepOffsetHigher.position + Vector3.forward, stepOffsetHigher.TransformDirection(direction), out hitUpper, stepOffsetRayLengthUpper))
            {
                Vector3 forceStairs = new Vector3(0f, stepSmooth, 0f);
                playerRigidbody.AddForce(forceStairs * 10f, ForceMode.Acceleration);
            }
        }
    }


    // CHECK FUNCTIONS
    private void CheckGround()
    {
        var hitColliders = Physics.OverlapSphere(groundTransform.position, groundRadius, whatIsGround);
        isGrounded = hitColliders.Length > 0;
    }
    
    private void CheckFalling()
    {
        float currentVel = playerRigidbody.velocity.y;
        if (lastFramePosition > currentVel)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        lastFramePosition = playerRigidbody.velocity.y;
    }

    // COOLDOWN RESETS

    private void ResetJump()
    {
        readyToJump = true;
        TrailJumpFeedbackReset();
    }
    private void ResetDoubleJump()
    {
        isDoubleJumping = false;
        TrailJumpFeedbackReset();
    }

    // GIZMOS -- EDITOR SETTINGS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundTransform.position, groundRadius);
    }
    
    // FEEDBACK
    
    public void TrailJumpFeedback()
    {
        trailLeftShoe.emitting = true;
        trailRightShoe.emitting = true;

        trailLeftShoe.widthMultiplier = 0.30f;
        trailRightShoe.widthMultiplier = 0.30f;

        trailLeftShoe.time = 0.3f;
        trailRightShoe.time = 0.3f;
    }

    public void TrailJumpFeedbackReset()
    {
        trailLeftShoe.emitting = true;
        trailRightShoe.emitting = true;

        trailLeftShoe.widthMultiplier = 0.10f;
        trailRightShoe.widthMultiplier = 0.10f;

        trailLeftShoe.time = 0.04f;
        trailRightShoe.time = 0.04f;
    }
}