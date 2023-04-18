using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public enum EPlayerModeState { REGULAR, AIMING, COMBAT, COMPANION_TRANSFORMATION }

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    
    [Header("References")]
    [SerializeField] public IInteractuable currentInteraction;
    
    [Header("Settings")]
    [SerializeField] private int health;
    [SerializeField] public EPlayerModeState modeState = EPlayerModeState.REGULAR;
    [SerializeField] CameraManager cameraManager;
    
    //Inputs
    [HideInInspector] public PlayerInputController myInputs;

    //Feedback
    [HideInInspector] public PlayerFeedbackController feedbackController;
    
    [HideInInspector] public Rigidbody playerRigidbody;

    // Internal variables
    private bool justReceivedDamage = false;
    private bool godMode = false;

    // Constant variables
    private const float justReceivedDamageTimer = 0.25f;
    
    // Start
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
        feedbackController = GetComponent<PlayerFeedbackController>();
        
        myInputs.OnInteractPerformed += DoInteract;
        
        Health = health;
        modeState = EPlayerModeState.COMBAT;
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
            BlockInputsDamage();
            
            // Apply operations
            Health -= damageAmount;
            feedbackController.PlayReceiveDamageFeedback();
            justReceivedDamage = true;

            // Reset timer to receive damage
            Invoke(nameof(ResetJustReceivedDamage), justReceivedDamageTimer);

            if(Health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        return modeState == EPlayerModeState.REGULAR || modeState == EPlayerModeState.AIMING;
    }

    public bool CanMove()
    {
        return modeState == EPlayerModeState.REGULAR;
    }

    public bool CanJump()
    {
        return modeState == EPlayerModeState.REGULAR;
    }
    
    public bool CanAttack()
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
                break;
            case EPlayerModeState.AIMING:
                break;
            case EPlayerModeState.COMBAT:
                break;
            case EPlayerModeState.COMPANION_TRANSFORMATION:
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
}