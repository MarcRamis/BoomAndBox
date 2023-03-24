using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    //[Header("Unity Events")]
    //[SerializeField] UnityEvent Death_Event;
    //[SerializeField] UnityEvent Restart_Event;

    // Start
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        myInputs = GetComponent<PlayerInputController>();
        playerFeedbackController = GetComponent<PlayerFeedbackController>();
        playerCharacterAnimations = GetComponent<PlayerCharacterAnimations>();
        
        myInputs.OnInteractPerformed += DoInteract;
        //if (Death_Event == null)
        //    Death_Event = new UnityEvent();
        //if(Restart_Event == null)
        //    Restart_Event = new UnityEvent();

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

            // Call event
            //Death_Event?.Invoke();
            JSON_Creator.Instance.PlayerDied();

            // Apply operations
            Health -= damageAmount;
            playerFeedbackController.PlayReceiveDamageFeedback();
            playerCharacterAnimations.PlayReceiveDamageAnimation();
            justReceivedDamage = true;

            // Reset timer to receive damage
            Invoke(nameof(ResetJustReceivedDamage), justReceivedDamageTimer);

            if(Health <= 0)
            {
                //Restart_Event?.Invoke();
                JSON_Creator.Instance.LevelRestart();
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
