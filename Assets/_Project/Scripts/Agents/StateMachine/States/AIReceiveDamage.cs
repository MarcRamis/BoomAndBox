using UnityEngine;
using UnityEngine.AI;

public class AIReceiveDamage : IAIState
{
    private MTimer knockedTimer;
    private const float knockedTime = 1f;
    private float knockBackForce = 15f;

    private bool isKnocked;

    // Enter the state: This method is called when entering the AIReceiveDamage state.
    // It sets isKnocked to true and creates a new MTimer with a time limit of knockedTime.
    // It subscribes to the OnTimerEnd event of the timer and starts the timer.
    public void Enter(Agent agent)
    {
        isKnocked = true;

        knockedTimer = new MTimer();
        knockedTimer.SetTimeLimit(knockedTime);
        knockedTimer.OnTimerEnd += OnKnockedTimeFinished;
        knockedTimer.StartTimer();
    }

    // Exit the state: This method is called when exiting the AIReceiveDamage state. No implementation is needed for this state.
    public void Exit(Agent agent)
    {
        // No implementation needed for this state
    }

    // GetId: Returns the unique identifier for this state, which is EAIState.RECEIVE_DAMAGE.
    public EAIState GetId()
    {
        return EAIState.RECEIVE_DAMAGE;
    }

    // Update the state: This method is called during the update loop.
    // It calls the LookAtPlayer method to make the agent look at the player.
    // If the agent is not knocked, it calls the OnUpdate method of the agent's manager, passing the agent as a parameter.
    public void Update(Agent agent)
    {
        LookAtPlayer(agent);

        knockedTimer.Update(Time.fixedDeltaTime);
        if (!isKnocked)
        {
            agent.manager.OnUpdate(agent);
        }
    }

    // OnKnockedTimeFinished: This method is called when the knockedTimer reaches its time limit.
    // It sets isKnocked to false, allowing the agent to resume normal behavior.
    private void OnKnockedTimeFinished()
    {
        isKnocked = false;
    }

    // LookAtPlayer: This method makes the agent look at the player.
    // It creates a new Vector3 with the same x, y, and z coordinates as the player's position.
    // It then calls the RotateTo method of the agent, passing the lockY vector and the rotationSpeed defined in the agent's configuration.
    private void LookAtPlayer(Agent agent)
    {
        Vector3 lockY = new Vector3(agent.player.transform.position.x, agent.player.transform.position.y, agent.player.transform.position.z);
        agent.RotateTo(lockY, agent.config.rotationSpeed);
    }
}
