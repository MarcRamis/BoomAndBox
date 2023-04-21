using UnityEngine;
using UnityEngine.AI;

public class AIReceiveDamage : IAIState
{
    private MTimer knockedTimer;
    private const float knockedTime = 1f;

    private bool isKnocked;

    public void Enter(Agent agent)
    {
        isKnocked = true;

        knockedTimer = new MTimer();
        knockedTimer.SetTimeLimit(knockedTime);
        knockedTimer.OnTimerEnd += OnKnockedTimeFinished;
        knockedTimer.StartTimer();
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
        knockedTimer.Update(Time.fixedDeltaTime);
        if (!isKnocked)
        {
            agent.manager.OnUpdate(agent);
        }
    }
    private void OnKnockedTimeFinished()
    {
        isKnocked = false;
    }
}
