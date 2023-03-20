using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    
    [Header("References")]
    [SerializeField] public IInteractuable currentInteraction;

    [Header("Settings")]
    [SerializeField] private int health;

    //Inputs
    [HideInInspector] public PlayerInputController myInputs;

    //Feedback
    [HideInInspector] private PlayerFeedbackController playerFeedbackController;
    [HideInInspector] private PlayerCharacterAnimations playerCharacterAnimations;

    [HideInInspector] public Rigidbody playerRigidbody;

    // Internal variables
    private bool justReceivedDamage = false;
    private bool godMode = false;

    // Constant variables
    private const float justReceivedDamageTimer = 0.25f;
    
    // Start
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
        playerFeedbackController = GetComponent<PlayerFeedbackController>();
        playerCharacterAnimations = GetComponent<PlayerCharacterAnimations>();
        
        myInputs.OnInteractPerformed += DoInteract;
        
        Health = health;        
    }
    
    // Update
    void Update()
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
            BlockInputsToAllow();
            
            // Apply operations
            Health -= damageAmount;
            playerFeedbackController.PlayReceiveDamageFeedback();
            playerCharacterAnimations.PlayReceiveDamageAnimation();
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

    public void BlockInputs()
    {
        playerRigidbody.velocity = Vector3.zero;
        myInputs.DisableGameActions();
    }

    public void AllowInputs()
    {
        myInputs.EnableGameActions();
    }

    public void BlockInputsToAllow()
    {
        BlockInputs();
        Invoke(nameof(AllowInputs), 0.8f);
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
