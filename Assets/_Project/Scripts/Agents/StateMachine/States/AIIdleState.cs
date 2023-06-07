using UnityEngine;
using UnityEngine.AI;

public class AIIdleState : IAIState
{
    public void Enter(Agent agent)
    {
        agent.manager.OnEnter(agent);
    }

    public void Exit(Agent agent)
    {
    }
    
    public EAIState GetId()
    {
        return EAIState.IDLE;
    }
    
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);
    }
}
