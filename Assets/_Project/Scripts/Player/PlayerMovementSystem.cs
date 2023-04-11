using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;

public enum EMoveState { WALKING, DASHING, AIMING, SLIDING, AIR, COMBAT }
public enum EAnimState { IDLE, RUNNING }

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform model;
    [SerializeField] private Camera mainCamera;
    [HideInInspector] public Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider;
    [HideInInspector] public Player playerScript;
    
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
    [HideInInspector] public Vector3 moveDirection;
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
    [SerializeField] public Transform orientation;
    [SerializeField] public Transform fullOrientation;
    [SerializeField] private float modelRotationSpeed;
    [SerializeField] private float modelRotationAimSpeed;
    [SerializeField] public Transform lookAt;

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
    // this variable exists because setting to true fall instantly wasn't pretty satisfying
    // Also it helps to "hide" partially a problem when u are grounded and sloping on a surface
    [HideInInspector] private float timeToSetFall = 0.15f;
    [SerializeField] private float fallingthreshold = 0.5f;

    //Inputs
    [HideInInspector] public PlayerInputController myInputs;

    [Header("Ground Check")]
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundTransform;
    [HideInInspector] public bool isGrounded;

    //Feedback
    [HideInInspector] private PlayerFeedbackController playerFeedbackController;
    
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
        /* References into the scripts, variable initialization 
         * Reference to components
         * (Important) This method is executed although the script is disabled*/
        
        playerScript = GetComponent<Player>();
        playerRigidbody = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
        playerFeedbackController = GetComponent<PlayerFeedbackController>();
        
        // Initialize inputs
        myInputs.OnJumpPerformed += DoJump;        
        
        // Initalize properties
        playerRigidbody.freezeRotation = true;
        readyToJump = true;
        currentDoubleJumps = doubleJumpCounter;
    }

    private void Start()
    {
        /* Executed before the first frame and sonly if the script is enabled
         * Here goes: delays, enemy movement, coroutines*/
    }

    private void Update()
    {
        /* Executed one time per frame. 
         * It doesn't depends on the machine is working.
         * Here goes inputs and variable updates. */
        

        CheckGround();

        CheckFalling();

        MyInputDirection();
        StartCoyoteTime();
        SpeedControl();
        HandleMovementState();
        HandleAnimState();

        // Handle drag
        HandleDrag();
    }

    private void FixedUpdate()
    {
        /* Executed every x seconds (0.2 usually)
         * It doesn't depends on the machine is being executed.
         * Here goes: physics movement*/


        MovePlayer();
        RotateModel();
        //StepOffset();
        
        OnLand();
    }
    private void LateUpdate()
    {
        /* Executed after all updates
         * Ued to control the camera movement*/
    }

    // Functions
    private void HandleDrag()
    {
        if (movementState == EMoveState.WALKING || (movementState == EMoveState.AIMING && isGrounded) || isDoubleJumping)
            playerRigidbody.drag = groundDrag;
        else
            playerRigidbody.drag = 0;
    }
    private void MyInputDirection()
    {
        // Take input directions
        horizontalInput = myInputs.moveDirection.x;
        verticalInput = myInputs.moveDirection.y;
    }
    private void StartCoyoteTime()
    {
        if (isGrounded) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
    }

    private void DoJump()
    {
        if (playerScript.CanJump())
        {
            // Jump on ground
            // Only can jump when is grounded or in coyote time
            if ((readyToJump && isGrounded) || (readyToJump && coyoteTimeCounter > 0f))
            {
                coyoteTimeCounter = 0f;
                readyToJump = false;

                ApplyJumpForce();
                playerFeedbackController.PlayJumpFeedback();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            // Double Jump in air
            /* Only can jump when jump cooldown is ready to prevent the double jump spam.
             * There is a counter of double jumps in air the player can make to change if it's necessary.

             * Coyote time is applied but not really necessary. Only to prevent the player doesn't double jump when in 
               because it mustn't count.*/
            else if (readyToJump && landing && currentDoubleJumps > 0 && !isDashing && coyoteTimeCounter <= 0f)
            {
                isDoubleJumping = true;

                ApplyJumpForce();
                playerFeedbackController.PlayDoubleJumpFeedback();

                currentDoubleJumps--;
                timeInAir = 0;

                Invoke(nameof(ResetDoubleJump), jumpCooldown);
            }
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
                playerFeedbackController.PlayLandingLargeFeedback();
            }
            else if (timeInAir >= middleTimeLanding)
            {
                playerFeedbackController.PlayLandingShortFeedback();
            }
            else if (timeInAir >= lowTimeLanding)
            {
                playerFeedbackController.PlayLandingShortFeedback();
            }
            else if (timeInAir >= veryLowTimeLanding)
            {
                playerFeedbackController.PlayLandingShortFeedback();
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
        // Calculate the view direction from the player to the main camera
        Vector3 viewDirFullOrientation = transform.position - mainCamera.transform.position;
        fullOrientation.forward = viewDirFullOrientation.normalized;

        if (!isAiming)
        {
            // Transform only with orientation on x, z. Needed to just rotate the player in the input direction
            // but i use it to move the player to the camera direction

            Vector3 viewDir = transform.position - new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
            orientation.forward = viewDir.normalized;

            Vector3 inputDir = Vector3.zero;
            // checking not dashing because i dont want to get input rotation while dashing
            if (!isDashing)
            {
                // Calculate the input direction based on the orientation of the player and the user's input
                inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            }
            else
            {
                inputDir = orientation.forward;
            }

            // If the input direction is non-zero, rotate the model in the direction of the input
            if (inputDir != Vector3.zero)
            {
                model.forward = Vector3.Slerp(model.forward, inputDir.normalized, Time.fixedDeltaTime * modelRotationSpeed);
            }
        }
        else
        {
            Vector3 dirToCombatLookAt = lookAt.position - new Vector3(mainCamera.transform.position.x, lookAt.position.y, mainCamera.transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            model.forward = Vector3.Slerp(model.forward, dirToCombatLookAt.normalized, Time.fixedDeltaTime * modelRotationAimSpeed);
        }

        //CalculateNormal();
    }

    private void CalculateNormal()
    {
        // Search for the surface normal to rotate the model on slopes
        RaycastHit hit;
        if (Physics.Raycast(groundTransform.position, groundTransform.TransformDirection(-Vector3.up), out hit, 1.0f))
        {
            Vector3 surfaceNormal = hit.normal;

            if (!isGrounded) surfaceNormal = Vector3.up;

            Quaternion targetRotation = Quaternion.FromToRotation(model.up, surfaceNormal) * model.rotation;
            model.rotation = Quaternion.Slerp(model.rotation, targetRotation, modelRotationSpeed * Time.fixedDeltaTime);
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
        if(!isGrounded)
        {
            float currentVel = playerRigidbody.velocity.y;
            if (lastFramePosition > currentVel)
            {
                Invoke(nameof(SetFalling), timeToSetFall);
            }

            //float distance = lastFramePosition - currentVel;
            //Debug.Log(distance);

            //if(distance > fallingthreshold)
            //{
            //    Invoke(nameof(SetFalling), timeToSetFall);
            //}
            //else
            //{
            //    isFalling = false;
            //}
            
        }
        lastFramePosition = playerRigidbody.velocity.y;
    }

    // COOLDOWN RESETS
    private void SetFalling()
    {
        isFalling = true;
    }

    private void ResetJump()
    {
        readyToJump = true;
        playerFeedbackController.StopJumpFeedback();
    }
    private void ResetDoubleJump()
    {
        isDoubleJumping = false;
        playerFeedbackController.StopDoubleJumpFeedback();
    }

    // GIZMOS -- EDITOR SETTINGS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundTransform.position, groundRadius);
    }
    
    // FEEDBACK
}