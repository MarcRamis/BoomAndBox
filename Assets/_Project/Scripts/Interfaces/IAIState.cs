public enum EAIState
{
    RANDOM_WALK,
    CHASE_PLAYER,
    CHARGE
}

public interface IAIState
{
    EAIState GetId();

    void Enter(Agent agent);
    void Update(Agent agent);
    void Exit(Agent agent);
}
