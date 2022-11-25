public enum AiBehaviour
{
    ORBIT,
    CHASE_PLAYER,
    DRON_CHARGE
}

public interface IAIState
{
    AiBehaviour GetId();

    void Enter(Agent agent);
    void Update(Agent agent);
    void Exit(Agent agent);
}
