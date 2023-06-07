using UnityEngine;
using UnityEngine.AI;

public class AIRandomWalkState : IAIState
{
    private Vector3 currentTarget; // The current target position for random walking
    private float timer = 0f; // Timer for changing the target position

    // Enter the state: This method is called when entering the AIRandomWalkState.
    // It sets the agent's navigation mesh speed and angular speed to the idle values defined in the agent's configuration.
    // It also generates a random target position on the NavMesh for the agent to walk towards.
    public void Enter(Agent agent)
    {
        agent.navMesh.speed = agent.config.idleSpeed;
        agent.navMesh.angularSpeed = agent.config.idleAngularSpeed;

        currentTarget = RandomNavmeshLocation(agent, agent.config.maxRandomWalkRadius);
    }

    // Exit the state: This method is called when exiting the AIRandomWalkState. No implementation is needed for this state.
    public void Exit(Agent agent)
    {
        // No implementation needed for this state
    }

    // GetId: Returns the unique identifier for this state, which is EAIState.RANDOM_WALK.
    public EAIState GetId()
    {
        return EAIState.RANDOM_WALK;
    }

    // Update the state: This method is called during the update loop. It calls the OnUpdate method of the agent's manager, passing the agent as a parameter.
    // If the agent is currently on the navigation mesh, it plays the walk animation through the agent's feedbackController and follows a random path.
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);

        if (agent.navMesh.isOnNavMesh)
        {
            agent.feedbackController.PlayWalk();
            FollowRandomPath(agent);
        }
    }

    // Follow a random path: This method is responsible for changing the target position for random walking.
    // It decreases the timer and if it reaches zero, it generates a new random target position on the NavMesh.
    // It then updates the agent's destination to the new target position.
    private void FollowRandomPath(Agent agent)
    {
        timer -= Time.fixedDeltaTime;

        if (timer < 0.0f)
        {
            currentTarget = RandomNavmeshLocation(agent, agent.config.maxRandomWalkRadius);

            timer = agent.config.maxTimeRandomWalk;
        }

        agent.ChangeTargetDestination(currentTarget);
    }

    // Get a random position on the NavMesh: This method generates a random direction within the specified radius around the agent.
    // It adds the random direction to the agent's current position and uses NavMesh.SamplePosition to find a valid position on the NavMesh within the radius.
    // It then returns the final position.
    // This implementation was based on a solution found on the Unity Answers forum: https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
    public Vector3 RandomNavmeshLocation(Agent agent, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += agent.transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }
}
