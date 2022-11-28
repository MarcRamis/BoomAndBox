using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Agent, IDamageable
{
    [Header("Settings")]
    [SerializeField] protected int health;
    public int Health { get; set; }
    protected GameObject player;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [HideInInspector] public bool isGrounded;

    // Internal variables
    private bool justReceivedDamage = false;

    // Constant variables
    private const float justReceivedDamageTimer = 0.15f;
    private const float gravityAddition = 2.0f;

    // Awake
    protected new void Awake()
    {
        base.Awake();
        Init();
    }
    protected new void Start()
    {
        base.Start();
    }

    // Update
    protected new void Update()
    {
        base.Update();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        HandleGravity();
    }
    protected new void FixedUpdate()
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
        if (!justReceivedDamage)
        {
            Health -= damageAmount;

            justReceivedDamage = true;
            Invoke(nameof(ResetJustReceivedDamage), justReceivedDamageTimer);
        }
        
        if (Health <= 0)
        {
            OnDeath();
        }
    }

    private void ResetJustReceivedDamage()
    {
        justReceivedDamage = false;
    }
    
    public virtual void OnDeath()
    {
    }

    // Functions
    private void HandleGravity()
    {
        if (!isGrounded)
        {
            rigidbody.velocity += new Vector3(0f, -gravityAddition, 0f);
        }
        else
        {

        }
    }
}
