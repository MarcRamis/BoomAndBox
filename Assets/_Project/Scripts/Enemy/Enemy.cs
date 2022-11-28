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
    private const float gravityAddition = 1.0f;

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

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        HandleGravity();
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
