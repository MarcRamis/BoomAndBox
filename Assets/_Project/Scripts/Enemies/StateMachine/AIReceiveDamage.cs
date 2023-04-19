using UnityEngine;
using UnityEngine.AI;

public class AIReceiveDamage : IAIState
{
    public void Enter(Agent agent)
    {
    }

    public void Exit(Agent agent)
    {
    }

    public EAIState GetId()
    {
        return EAIState.RECEIVE_DAMAGE;
    }
    
    public void Update(Agent agent)
    {
    }
}
