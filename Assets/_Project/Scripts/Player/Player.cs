using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    
    [Header("Settings")]
    [SerializeField] private int health;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks receiveDamageFeedback;

    // Internal variables
    private bool justReceivedDamage = false;

    // Constant variables
    private const float justReceivedDamageTimer = 0.25f;

    // Start
    void Start()
    {
        Health = health;
    }

    // Update
    void Update()
    {
        
    }

    // Functions
    public void Damage(int damageAmount)
    {
        if (!justReceivedDamage)
        {
            // Apply operations
            Health -= damageAmount;
            receiveDamageFeedback.PlayFeedbacks();
            justReceivedDamage = true;

            // Reset timer to receive damage
            Invoke(nameof(ResetJustReceivedDamage), justReceivedDamageTimer);
        }
    }
    private void ResetJustReceivedDamage()
    {
        justReceivedDamage = false;
    }

}
