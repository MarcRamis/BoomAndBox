public class AIKeepDistance : IAIState
{
    public void Enter(Agent agent)
    {
    }

    public void Exit(Agent agent)
    {
        agent.manager.OnUpdate(agent);
    }
    
    public EAIState GetId()
    {
        return EAIState.KEEP_DISTANCE;
    }

    public void Update(Agent agent)
    {
    }
}
