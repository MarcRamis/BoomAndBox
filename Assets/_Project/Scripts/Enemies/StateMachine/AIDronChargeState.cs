using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronChargeState : IAIState
{
    // Internal variables
    private float timerLoop;
    private GameObject chargePrefab;

    public void Enter(Agent agent)
    {
        agent.feedbackController.PlayPreparingCharge();
        
        timerLoop = agent.config.timePreparingToCharge;
        
        DesactivatePhysiscs(agent);
    }

    public void Exit(Agent agent)
    {
        agent.feedbackController.StopPreparingCharge();
        agent.feedbackController.StopCharge();
 
        ActivatePhysics(agent);
    }

    public EAIState GetId()
    {
        return EAIState.CHARGE;
    }

    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);
        
        if (agent.config.isCharging)
        {
            timerLoop -= Time.fixedDeltaTime;

            agent.feedbackController.StopCharge();
            
            if (agent.rigidbody.velocity.magnitude <= 0.1f && timerLoop < 0.0f)
            {
                agent.feedbackController.StopPreparingCharge();
                ActivatePhysics(agent);
            }
        }
        else
        {
            agent.feedbackController.PlayPreparingCharge();
            agent.feedbackController.PlayCharge();

            if (timerLoop < 0.0f)
            {
                DesactivatePhysiscs(agent);
                Charge(agent);
                timerLoop = agent.config.timePreparingToCharge;
            }
        }
    }

    private void Charge(Agent agent)
    {
        Vector3 direction = agent.player.transform.position - agent.transform.position;
        direction = direction.normalized;
        Vector3 forceToApply = direction * agent.config.chargeForce;
        
        agent.rigidbody.AddForce(forceToApply, ForceMode.VelocityChange);
        agent.transform.rotation = Quaternion.identity;
    }

    private void DesactivatePhysiscs(Agent agent)
    {
        agent.config.isCharging = true;
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
        agent.transform.rotation = Quaternion.identity;
    }
    
    private void ActivatePhysics(Agent agent)
    {
        agent.config.isCharging = false;
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
        agent.navMesh.angularSpeed = 10000;
        agent.transform.rotation = Quaternion.identity;
    }
}