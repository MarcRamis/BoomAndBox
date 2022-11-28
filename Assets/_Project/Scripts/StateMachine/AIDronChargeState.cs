using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronChargeState : IAIState
{
    // Internal variables
    private Transform player;
    private float timerLoop;
    private bool isCharging;
    private GameObject chargePrefab;

    public void Enter(Agent agent)
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback != null)
            agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback.PlayFeedbacks();


        timerLoop = agent.config.timePreparingToCharge;
        
        DesactivatePhysiscs(agent);
    }

    public void Exit(Agent agent)
    {
        if (agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback != null)
            agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback.StopFeedbacks();

        if (agent.gameObject.GetComponent<Dron>().chargeFeedback != null)
            agent.gameObject.GetComponent<Dron>().chargeFeedback.StopFeedbacks();

        ActivatePhysics(agent);
    }

    public EAIState GetId()
    {
        return EAIState.CHARGE;
    }

    public void Update(Agent agent)
    {
        if (IsPlayerFarAway(agent))
        {
            agent.stateMachine.ChangeState(EAIState.CHASE_PLAYER);
            return;
        }
        
        if (isCharging)
        {
            timerLoop -= Time.fixedDeltaTime;
            
            if (agent.gameObject.GetComponent<Dron>().chargeFeedback != null)
                agent.gameObject.GetComponent<Dron>().chargeFeedback.StopFeedbacks();

            if (agent.rigidbody.velocity.magnitude <= 0.1f && timerLoop < 0.0f)
            {
                if (agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback != null)
                    agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback.StopFeedbacks();

                ActivatePhysics(agent);
            }
        }
        else
        {
            if (agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback != null)
                agent.gameObject.GetComponent<Dron>().preparingForChargeFeedback.PlayFeedbacks();
            
            if (agent.gameObject.GetComponent<Dron>().chargeFeedback != null)
                agent.gameObject.GetComponent<Dron>().chargeFeedback.PlayFeedbacks();

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
        Vector3 direction = player.transform.position - agent.transform.position;
        direction = direction.normalized;
        Vector3 forceToApply = direction * agent.config.chargeForce;
        
        agent.rigidbody.AddForce(forceToApply, ForceMode.VelocityChange);
    }

    private bool IsPlayerFarAway(Agent agent)
    {
        return Vector3.Distance(player.transform.position, agent.transform.position) > agent.config.maxDistanceToChaseState;
    }

    private void DesactivatePhysiscs(Agent agent)
    {
        isCharging = true;
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
    }

    private void ActivatePhysics(Agent agent)
    {
        isCharging = false;
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
        agent.navMesh.angularSpeed = 10000;
    }
}