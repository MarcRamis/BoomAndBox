using UnityEngine;
using UnityEngine.AI;

public class AIReceiveDamage : IAIState
{
    private MTimer knockedTimer;
    private const float knockedTime = 1f;
    private float knockBackForce = 15f;

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
        LookAtPlayer(agent);

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

    private void LookAtPlayer(Agent agent)
    {
        Vector3 lockY = new Vector3(agent.player.transform.position.x, agent.player.transform.position.y, agent.player.transform.position.z);
        agent.RotateTo(lockY, agent.config.rotationSpeed);
    }
}
