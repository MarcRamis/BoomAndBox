using UnityEngine;
using UnityEngine.AI;

public class AISeekState : IAIState
{
    public void Enter(Agent agent)
    {
        agent.navMesh.speed = agent.config.chaseSpeed;
        agent.navMesh.angularSpeed = agent.config.chaseAngularSpeed;

        agent.manager.OnEnter(agent);
    }
    
    public void Exit(Agent agent)
    {
    }

    public EAIState GetId()
    {
        return EAIState.SEEK;
    }

    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);

        if (agent.navMesh.isOnNavMesh)
        {
            agent.feedbackController.PlayWalk();       
        }
    }
}