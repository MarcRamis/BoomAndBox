using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronChargeState : IAIState
{
    private Transform player;
    private float timerLoop;
    private bool isCharging;
    private GameObject chargePrefab;
    
    public void Enter(Agent agent)
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (agent.gameObject.GetComponent<Dron>().chargePrefab != null)
            agent.gameObject.GetComponent<Dron>().chargePrefab.SetActive(true);
        
        isCharging = true;
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;

        timerLoop = agent.config.timePreparingToCharge;
    }

    public void Exit(Agent agent)
    {
        if (agent.gameObject.GetComponent<Dron>().chargePrefab != null)
            agent.gameObject.GetComponent<Dron>().chargePrefab.SetActive(false);

        isCharging = false;
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
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
            
            if (agent.rigidbody.velocity.magnitude <= 0.1f && timerLoop < 0.0f)
            {
                if (agent.gameObject.GetComponent<Dron>().chargePrefab != null)
                    agent.gameObject.GetComponent<Dron>().chargePrefab.SetActive(false);

                isCharging = false;
                agent.rigidbody.isKinematic = true;
                agent.navMesh.enabled = true;
            }
        }
        else
        {
            if (agent.gameObject.GetComponent<Dron>().chargePrefab != null)
                agent.gameObject.GetComponent<Dron>().chargePrefab.SetActive(true);

            if (timerLoop < 0.0f)
            {
                Charge(agent);
                timerLoop = agent.config.timePreparingToCharge;
            }
        }

    }

    private void Charge(Agent agent)
    {
        isCharging = true;
        agent.rigidbody.isKinematic = false;
        agent.navMesh.enabled = false;
        
        Vector3 direction = player.transform.position - agent.transform.position;
        direction = direction.normalized;
        Vector3 forceToApply = direction * agent.config.chargeForce;
        
        agent.rigidbody.AddForce(forceToApply, ForceMode.VelocityChange);
    }

    private bool IsPlayerFarAway(Agent agent)
    {
        return Vector3.Distance(player.transform.position, agent.transform.position) > agent.config.maxDistanceToChaseState;
    }
}