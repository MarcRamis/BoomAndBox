using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;

public class Dron : Enemy
{
    /// <summary>
    /// How the Dron works!
    /// Starts in IDLE state where he's orbiting around something.
    /// 
    /// If he sees the player start the CHASING state until he reach a lower distance
    /// Then starts the PREPARING state where wait for a little time and then make the CHARGE state,
    /// it's an impulse against the player
    /// </summary>

    // Settings
    [SerializeField] private float distanceToFollowPlayer;
    [SerializeField] private float distanceToLosePlayer;
    [SerializeField] private float distanceToStartCharging;
    [SerializeField] private float timeCharging;
    [SerializeField] private float forceCharge;
    [SerializeField] private float moveSpeed;

    [Header("References")]
    [SerializeField] private GameObject idleTarget;
    private GameObject followTarget;
    [SerializeField] private MMAutoRotate mMAutoRotate;

    // Constant variables

    // Internal variables
    private Rigidbody m_Rb;
    private bool isCharging;

    // Start
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        followTarget = player;
        mMAutoRotate.OrbitCenterTransform = idleTarget.transform;
    }

    // Update 
    void Update()
    {
        ManageStates();
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < distanceToStartCharging)
        {
            if (!isCharging)
            {
                enemyState = EEnemyState.PREPARING;
                isCharging = true;
                Invoke(nameof(DoCharge), timeCharging);
            }
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < distanceToFollowPlayer)
        {
            enemyState = EEnemyState.CHASE_PLAYER;
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < distanceToLosePlayer)
        {
            enemyState = EEnemyState.IDLE;
        }
    }

    private void ManageStates()
    {
        switch (enemyState)
        {
            case EEnemyState.IDLE:
                
                mMAutoRotate.enabled = true;
                
                break;
            case EEnemyState.CHASE_PLAYER:
                
                mMAutoRotate.enabled = false;
                m_Rb.velocity = Seek(player.transform.position) * moveSpeed * Time.fixedDeltaTime;

                break;
            case EEnemyState.PREPARING:

                break;
            case EEnemyState.CHARGE:

                break;
        }
    }
    
    private void DoCharge()
    {
        enemyState = EEnemyState.CHARGE;
        Charge();
    }

    private void Charge()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction = direction.normalized;
        Vector3 forceToApply = direction * forceCharge;

        m_Rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
    }

    public override void OnDeath()
    {
        base.OnDeath();


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToFollowPlayer);
    }

    private Vector3 Seek(Vector3 _target)
    {
        Vector3 direction = _target - transform.position;
        direction = direction.normalized;

        return direction;
    }

    private Vector3 Flee(Vector3 _target)
    {
        Vector3 direction = transform.position - _target;
        direction = direction.normalized;

        return direction;
    }
}