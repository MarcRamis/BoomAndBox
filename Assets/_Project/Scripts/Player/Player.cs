using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    
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
        Health = health;
    }

    // Update
    void Update()
    {
        
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
}
