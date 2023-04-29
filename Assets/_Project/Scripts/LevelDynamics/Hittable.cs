using System;
using UnityEngine;

public class Hittable : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private int health;
    public int Health { get; set; }

    [HideInInspector] private HittableFeedbackController feedbackController;
    
    private float justTakenDamageCd = 0.8f;
    private bool justTakenDamage = false;
    
    private void Awake()
    {
        Health = health;
        feedbackController = GetComponent<HittableFeedbackController>();
    }
    
    public void Damage(int damageAmount)
    {
        if (!justTakenDamage)
        {
            feedbackController.PlayTakeDamage();
            Health -= damageAmount;
            justTakenDamage = true; 
            Invoke(nameof(ResetDamage), justTakenDamageCd);
        }
        
        //Debug.Log(Health);
        
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    private void ResetDamage()
    {
        justTakenDamage = false;
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public void Knockback(float force)
    {
        //throw new NotImplementedException();
    }
}
