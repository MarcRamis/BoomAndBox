using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

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
    [SerializeField] private float timePreparing;
    [SerializeField] private float cooldownCharge;
    [SerializeField] private float forceCharge;
    [SerializeField] private float moveSpeed;

    [Header("References")]
    [SerializeField] private GameObject idleTarget;
    private GameObject followTarget;
    [SerializeField] private MMAutoRotate mMAutoRotate;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks preparingFeedback;
    [SerializeField] private MMFeedbacks chargeFeedback;

    // Constant variables

    // Internal variables
    private Rigidbody m_Rb;
    private bool isPreparing;

    // Start
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        followTarget = player;
        mMAutoRotate.OrbitCenterTransform = idleTarget.transform;
        mMAutoRotate.enabled = true;
    }

    // Update 
    void Update()
    {
    }
    private void FixedUpdate()
    {
        if (enemyState != EEnemyState.PREPARING && enemyState != EEnemyState.CHARGE)
        {
            // start preparing for charge
            if (Vector3.Distance(player.transform.position, transform.position) < distanceToStartCharging)
            {
                enemyState = EEnemyState.PREPARING;
                isPreparing = true;

                // Prepare for charge
                Invoke(nameof(DoCharge), timePreparing);
            }

            // chase player
            else if (Vector3.Distance(player.transform.position, transform.position) < distanceToFollowPlayer && !isPreparing)
            {
                mMAutoRotate.enabled = false;
                enemyState = EEnemyState.CHASE_PLAYER;
                m_Rb.velocity = Seek(player.transform.position) * moveSpeed * Time.fixedDeltaTime;
            }

            // idle
            else if (Vector3.Distance(player.transform.position, transform.position) < distanceToLosePlayer && !isPreparing)
            {
                mMAutoRotate.enabled = true;
                enemyState = EEnemyState.IDLE;
            }
        }
        // preparing state
        else if (enemyState == EEnemyState.PREPARING)
        {
            m_Rb.velocity = Vector3.zero;
            preparingFeedback.PlayFeedbacks();
        }
    }

    private void ManageStates()
    {
        switch (enemyState)
        {
            case EEnemyState.IDLE:
                
                
                
                break;
            case EEnemyState.CHASE_PLAYER:

               

                break;
            case EEnemyState.PREPARING:

                

                break;
            case EEnemyState.CHARGE:

                break;
        }
    }
    
    private void DoCharge()
    {
        // cooldown?

        enemyState = EEnemyState.CHARGE;

        Debug.Log("Charge");
        chargeFeedback.PlayFeedbacks();
        Charge();
    }

    private void Charge()
    {
        // calc force
        Vector3 direction = player.transform.position - transform.position;
        direction = direction.normalized;
        Vector3 forceToApply = direction * forceCharge;
        m_Rb.AddForce(forceToApply, ForceMode.Impulse);
        
        // check new states
        Invoke(nameof(ResetState), cooldownCharge);
    }
    
    private void ResetState()
    {
        enemyState = EEnemyState.IDLE;
        isPreparing = false;
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