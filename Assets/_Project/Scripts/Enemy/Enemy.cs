using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyState
{
    IDLE,
    FOLLOW_TARGET,
    CHARGING,
    CHARGE
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] protected int health;
    public int Health { get; set; }
    [SerializeField] protected EEnemyState enemyState;

    protected GameObject player; 

    // Awake
    void Awake()
    {
        Init();
    }

    // Update
    void Update()
    {
        
    }
    
    public virtual void Init()
    {
        Health = health;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void Damage(int damageAmount)
    {
        Health -= damageAmount;
        
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {

    }
}
