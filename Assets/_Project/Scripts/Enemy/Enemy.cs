using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Agent, IDamageable
{
    [Header("Settings")]
    [SerializeField] protected int health;
    public int Health { get; set; }
    protected GameObject player; 

    // Awake
    protected void Awake()
    {
        base.Awake();
        Init();
    }
    protected void Start()
    {
        base.Start();
    }

    // Update
    protected void Update()
    {
        base.Update();
    }
    protected void FixedUpdate()
    {
        base.FixedUpdate();
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
        Debug.Log("Die");
    }
}
