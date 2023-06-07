using UnityEngine;
using UnityEngine.AI;

public class AISeekState : IAIState
{
    // Enter the state: This method is called when entering the AISeekState.
    // It sets the agent's navigation mesh speed and angular speed to the chase values defined in the agent's configuration.
    // It then calls the OnEnter method of the agent's manager, passing the agent as a parameter.
    public void Enter(Agent agent)
    {
        agent.navMesh.speed = agent.config.chaseSpeed;
        agent.navMesh.angularSpeed = agent.config.chaseAngularSpeed;

        agent.manager.OnEnter(agent);
    }

    // Exit the state: This method is called when exiting the AISeekState. No implementation is needed for this state.
    public void Exit(Agent agent)
    {
        // No implementation needed for this state
    }

    // GetId: Returns the unique identifier for this state, which is EAIState.SEEK.
    public EAIState GetId()
    {
        return EAIState.SEEK;
    }

    // Update the state: This method is called during the update loop. It calls the OnUpdate method of the agent's manager, passing the agent as a parameter.
    // If the agent is currently on the navigation mesh, it plays the walk animation through the agent's feedbackController.
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);

        if (agent.navMesh.isOnNavMesh)
        {
            agent.feedbackController.PlayWalk();
        }
    }
}