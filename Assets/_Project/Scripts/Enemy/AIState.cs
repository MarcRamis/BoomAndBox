public enum AIStateID
{
    IDLE,
    DEATH
}

public interface AIState
{
    AIStateID GetId();

    void Enter(Agent agent);
    void Update(Agent agent);
    void Exit(Agent agent);
}
