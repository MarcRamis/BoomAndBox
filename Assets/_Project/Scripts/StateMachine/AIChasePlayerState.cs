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
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Update(Agent agent)
    {
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
}
