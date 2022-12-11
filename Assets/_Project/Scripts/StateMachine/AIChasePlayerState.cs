using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerState : IAIState
{
    private Transform player;
    private float timer = 0.0f;

    public EAIState GetId()
    {
        return EAIState.CHASE_PLAYER;
    }

    public void Enter(Agent agent)
    {
        agent.navMesh.speed = agent.config.chaseSpeed;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Update(Agent agent)
    {
        if (IsPlayerFarAway(agent))
        {
            agent.stateMachine.ChangeState(EAIState.RANDOM_WALK);
            return;
        }
        
        if (IsPlayerNear(agent))
        {
            agent.stateMachine.ChangeState(EAIState.CHARGE);
            return;
        }

        if (agent.navMesh.isOnNavMesh)
            CheckPathWithTime(agent);
    }

    public void Exit(Agent agent)
    {

    }

    private void CheckPathWithTime(Agent agent)
    {
        timer -= Time.fixedDeltaTime;

        if (timer < 0.0f)
        {
            float sqDistance = (player.position - agent.navMesh.destination).sqrMagnitude;
            if (sqDistance > agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.navMesh.destination = player.position;
            }
            timer = agent.config.maxTimeChase;
        }
    }

    private bool IsPlayerFarAway(Agent agent)
    {
        return Vector3.Distance(player.transform.position, agent.transform.position) > agent.config.maxDistanceToIdleState;
    }

    private bool IsPlayerNear(Agent agent)
    {
        return Vector3.Distance(player.transform.position, agent.transform.position) < agent.config.maxDistanceToChargeState;
    }
    private void ActivatePhysics(Agent agent)
    {
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
        agent.navMesh.angularSpeed = 10000;
        agent.transform.rotation = Quaternion.identity;
    }
}
