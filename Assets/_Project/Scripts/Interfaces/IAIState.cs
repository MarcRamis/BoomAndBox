public enum EAIState
{
    RANDOM_WALK,
    CHASE_PLAYER,
    CHARGE,
    RECEIVE_DAMAGE,
    KEEP_DISTANCE,
    IDLE,
    SEEK
}

public interface IAIState
{
    EAIState GetId();

    void Enter(Agent agent);
    void Update(Agent agent);
    void Exit(Agent agent);
}
