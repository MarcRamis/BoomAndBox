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
    /// 
    /// 
    /// </summary>
    
    // References
    [Header("Feedback")]
    [SerializeField] private MMFeedbacks damageFeedback;
    [SerializeField] private MMFeedbacks dieFeedback;
    [SerializeField] public MMFeedbacks preparingForChargeFeedback;
    [SerializeField] public MMFeedbacks chargeFeedback;
    [SerializeField] public GameObject explosionPrefab;
    [SerializeField] public GameObject model;

    // Constant variables

    // Internal variables
    [SerializeField] private LayerMask fallingMask;
    // Start

    private new void Start()
    {
        base.Start();
    }

    // Update 
    private new void Update()
    {
        base.Update();

        if(stateMachine.currentState == EAIState.CHASE_PLAYER || stateMachine.currentState == EAIState.RANDOM_WALK)
        {
            FallingCheck();
        }   
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
        damageFeedback.PlayFeedbacks();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        dieFeedback.PlayFeedbacks();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && rigidbody.velocity.magnitude > 0.1f)
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }
    private void DesactivatePhysiscs(Agent agent)
    {
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
        agent.transform.rotation = Quaternion.identity;
    }

    private void FallingCheck()
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, fallingMask))
        {
            DesactivatePhysiscs(this);
        }
    }

}