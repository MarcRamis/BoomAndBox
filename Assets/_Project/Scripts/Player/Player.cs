using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public enum EPlayerModeState { REGULAR, ONBOARDING, AIMING, COMBAT, COMPANION_TRANSFORMATION }

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    
    [Header("References")]
    [SerializeField] public IInteractuable currentInteraction;
    [SerializeField] public Transform model;
    [SerializeField] public Transform orientation;
    [SerializeField] public Transform fullOrientation;
    
    [Header("Settings")]
    [SerializeField] private int health;
    [SerializeField] public EPlayerModeState modeState = EPlayerModeState.REGULAR;
    [SerializeField] CameraManager cameraManager;
    [HideInInspector] public bool dashOnboarding = false;
    
    [HideInInspector] public ThrowingSystem throwingSystem;
    [HideInInspector] public CombatSystem combatSystem;

    //Inputs
    [HideInInspector] public PlayerInputController myInputs;

    //Feedback
    [HideInInspector] public PlayerFeedbackController feedbackController;
    
    [HideInInspector] public Rigidbody playerRigidbody;
    [HideInInspector] public Transform beingTargettedBy = null;

    // Internal variables
    private bool justReceivedDamage = false;
    private bool godMode = false;
    private bool canMove = true;

    // Constant variables
    private const float justReceivedDamageTimer = 0.25f;
    
    // Start
    private void Awake()
    {
        throwingSystem = GetComponent<ThrowingSystem>();
        combatSystem = GetComponent<CombatSystem>();
        playerRigidbody = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
        feedbackController = GetComponent<PlayerFeedbackController>();
        
        myInputs.OnInteractPerformed += DoInteract;
        
        Health = health;

        SetNewState(modeState);
        
    }

    // Update
    private void Update()
    {
    }

    private void DoInteract()
    {
        if (currentInteraction != null)
        {
            currentInteraction.MakeInteraction();
        }
    }
    
    // Functions
    public void Damage(int damageAmount)
    {
        if (!justReceivedDamage && !godMode)
        {
            BlockInputsWithTime(1f);
            
            // Apply operations
            Health -= damageAmount;
            Debug.Log(Health);
            feedbackController.PlayReceiveDamageFeedback();
            justReceivedDamage = true;

            // Reset timer to receive damage
            Invoke(nameof(ResetJustReceivedDamage), justReceivedDamageTimer);

            if(Health <= 0)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Health = health;

                if(EventsSystem.current != null)
                {
                    EventsSystem.current.PlayerDeath();
                }

            }
        }
    }
    private void ResetJustReceivedDamage()
    {
        justReceivedDamage = false;
    }
    public void SwitchGodMode()
    {
        godMode = !godMode;
        if(godMode)
        {
            Debug.Log("Invencibility ON");
        }
        else
        {
            Debug.Log("Invencibility OFF");
        }
    }
    
    public bool CanThrow()
    {
        return modeState == EPlayerModeState.REGULAR || modeState == EPlayerModeState.AIMING;
    }
    
    public bool CanDash()
    {
        return (modeState == EPlayerModeState.REGULAR || modeState == EPlayerModeState.AIMING ) && !dashOnboarding;
    }
    
    public bool CanMove()
    {
        return canMove;
    }

    public bool CanJump()
    {
        return modeState == EPlayerModeState.REGULAR || modeState == EPlayerModeState.ONBOARDING;
    }
    
    public bool CanCombat()
    {
        return modeState == EPlayerModeState.COMBAT;
    }

    public void SetNewState(EPlayerModeState newState)
    {
        modeState = newState;
        HandleModeState();
    }

    public void HandleModeState()
    {
        switch (modeState)
        {
            case EPlayerModeState.REGULAR:
                throwingSystem.YesMode();
                combatSystem.HideWeapon();
                break;
            case EPlayerModeState.AIMING:
                break;
            case EPlayerModeState.COMBAT:
                combatSystem.ShowWeapon();
                throwingSystem.NotMode();
                break;
            case EPlayerModeState.COMPANION_TRANSFORMATION:
                break;
            case EPlayerModeState.ONBOARDING:
                combatSystem.HideWeapon();
                break;
        }
    }

    public void BlockInputs()
    {
        playerRigidbody.velocity = Vector3.zero;
        myInputs.DisableGameActions();
    }

    public void BlockInputsAndCamera()
    {
        playerRigidbody.velocity = Vector3.zero;
        myInputs.DisableGameActions();
        cameraManager.LockCamera();
    }

    public void BlockMovement()
    {
        playerRigidbody.velocity = Vector3.zero;
        canMove = false;
    }

    public void AllowMovement()
    {
        canMove = true;
    }
    
    public void BlockMovementWithTime(float time)
    {
        BlockMovement();
        Invoke(nameof(AllowMovement), time);
    }

    public void AllowInputs()
    {
        myInputs.EnableGameActions();
        cameraManager.UnlockCamera();
    }


    public void BlockInputsDamage()
    {
        BlockInputs();
        Invoke(nameof(AllowInputs), 0.8f);
    }

    public void BlockInputsWithTime(float time)
    {
        BlockInputs();
        Invoke(nameof(AllowInputs), time);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractuable interactuable = other.gameObject.GetComponent<IInteractuable>();
        if (interactuable != null)
        {
            currentInteraction = interactuable;
            interactuable.InteractStarts();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        IInteractuable interactuable = other.gameObject.GetComponent<IInteractuable>();
        if (interactuable != null)
        {

            currentInteraction = null;
            interactuable.InteractEnds();
        }
    }

    public void Knockback(float force)
    {
        throw new System.NotImplementedException();
    }
}