using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerState : IAIState
{
    private float timer = 0.0f;

    public EAIState GetId()
    {
        return EAIState.CHASE_PLAYER;
    }

    public void Enter(Agent agent)
    {
        agent.playerScript.beingTargettedBy = agent.transform;
        agent.navMesh.speed = agent.config.chaseSpeed;
        agent.navMesh.angularSpeed = agent.config.chaseAngularSpeed;
    }
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);
        
        if (agent.navMesh.isOnNavMesh)
        { 
            agent.feedbackController.PlayRun();
            CheckPathWithTime(agent);
        }
    }
    
    public void Exit(Agent agent)
    {
    }

    private void CheckPathWithTime(Agent agent)
    {
        timer -= Time.fixedDeltaTime;

        if (timer < 0.0f)
        {
            float sqDistance = (agent.player.transform.position - agent.navMesh.destination).sqrMagnitude;
            if (sqDistance > agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.navMesh.destination = agent.player.transform.position;
            }
            timer = agent.config.maxTimeChase;
        }
    }

    private void ActivatePhysics(Agent agent)
    {
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
        agent.navMesh.angularSpeed = 10000;
        agent.transform.rotation = Quaternion.identity;
    }
}
