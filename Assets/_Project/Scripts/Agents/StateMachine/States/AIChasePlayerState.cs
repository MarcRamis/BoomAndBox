using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerState : IAIState
{
    private float timer = 0.0f;

    // GetId: Returns the unique identifier for this state, which is EAIState.CHASE_PLAYER.
    public EAIState GetId()
    {
        return EAIState.CHASE_PLAYER;
    }

    // Enter the state: This method is called when entering the AIChasePlayerState.
    // It sets the player's beingTargettedBy reference to the agent's transform.
    // It also sets the agent's navigation mesh speed and angular speed to the chase values defined in the agent's configuration.
    public void Enter(Agent agent)
    {
        agent.playerScript.beingTargettedBy = agent.transform;
        agent.navMesh.speed = agent.config.chaseSpeed;
        agent.navMesh.angularSpeed = agent.config.chaseAngularSpeed;
    }

    // Update the state: This method is called during the update loop.
    // It calls the OnUpdate method of the agent's manager, passing the agent as a parameter.
    // If the agent is on the navigation mesh, it plays the run animation through the agent's feedbackController and checks the path with time.
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);

        if (agent.navMesh.isOnNavMesh)
        {
            agent.feedbackController.PlayRun();
            CheckPathWithTime(agent);
        }
    }

    // Exit the state: This method is called when exiting the AIChasePlayerState. No implementation is needed for this state.
    public void Exit(Agent agent)
    {
        // No implementation needed for this state
    }

    // Check the path with time: This method is responsible for checking the agent's path periodically based on the timer.
    // It decreases the timer and if it reaches zero, it calculates the squared distance between the agent and the player.
    // If the squared distance is greater than the maxDistance squared defined in the agent's configuration,
    // it sets the agent's destination to the player's position.
    // It then resets the timer to the maxTimeChase value defined in the agent's configuration.
    private void CheckPathWithTime(Agent agent)
    {
        timer -= Time.fixedDeltaTime;

        if (timer < 0.0f)
        {
            float sqDistance = (agent.player.transform.position - agent.navMesh.destination).sqrMagnitude;
            if (sqDistance > agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.navMesh.destination = agent.player.transform.position;
            }
            timer = agent.config.maxTimeChase;
        }
    }
}