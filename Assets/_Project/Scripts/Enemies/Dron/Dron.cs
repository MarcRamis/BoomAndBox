using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class Dron : Enemy
{
    [SerializeField] private Transform wheelModel;
    [SerializeField] private float multiplierWheelSpeed;

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

        if (stateMachine.currentState == EAIState.CHASE_PLAYER || stateMachine.currentState == EAIState.RANDOM_WALK)
        {
            FallingCheck();
        }
        
        RotateModel();
    }

    private void RotateModel()
    {
        wheelModel.Rotate(Vector3.right * (navMesh.velocity.magnitude * multiplierWheelSpeed));
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);

        feedbackController.PlayTakeDamage();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        feedbackController.PlayDeath();
        Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && rigidbody.velocity.magnitude > 0.1f)
        {
            //collision.gameObject.GetComponent<IDamageable>().Damage(1);
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