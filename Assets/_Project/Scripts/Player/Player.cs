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
    [HideInInspector] PlayerMovementSystem pm;
    [HideInInspector] private Collider collider;

    [Header("Settings")]
    [SerializeField] private int health;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks receiveDamageFeedback;

    // Internal variables
    private bool justReceivedDamage = false;
    private bool godMode = false;

    // Constant variables
    private const float justReceivedDamageTimer = 0.25f;

    // Start
    void Start()
    {
        pm = GetComponent<PlayerMovementSystem>();
        Health = health;
        
        pm.myInputs.OnInteractPerformed += DoInteract;
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
        else
        {
            Debug.Log("No interaction availabel");
        }
    }

    // Functions
    public void Damage(int damageAmount)
    {
        if (!justReceivedDamage && !godMode)
        {
            // Apply operations
            Health -= damageAmount;
            receiveDamageFeedback.PlayFeedbacks();
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
