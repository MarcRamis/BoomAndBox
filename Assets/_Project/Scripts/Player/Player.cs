using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int Health { get; set; }
    
    [Header("Settings")]
    [SerializeField] private int health;

    // Start
    void Start()
    {
        Health = health;
    }

    // Update
    void Update()
    {
        
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;
    }

}
