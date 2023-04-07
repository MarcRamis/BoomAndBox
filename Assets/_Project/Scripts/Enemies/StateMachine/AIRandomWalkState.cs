using UnityEngine;
using UnityEngine.AI;

public class AIRandomWalkState : IAIState
{
    private Vector3 currentTarget;
    private float timer = 0f;

    public void Enter(Agent agent)
    {
        agent.navMesh.speed = agent.config.idleSpeed;

        currentTarget = RandomNavmeshLocation(agent, agent.config.maxRandomWalkRadius);
    }

    public void Exit(Agent agent)
    {
    }

    public EAIState GetId()
    {
        return EAIState.RANDOM_WALK;
    }
    
    public void Update(Agent agent)
    {
        agent.manager.OnUpdate(agent);

        if (agent.navMesh.isOnNavMesh)
            FollowRandomPath(agent);
    }

    private void FollowRandomPath(Agent agent)
    {
        timer -= Time.fixedDeltaTime;

        if (timer < 0.0f)
        {
            currentTarget = RandomNavmeshLocation(agent, agent.config.maxRandomWalkRadius);

            timer = agent.config.maxTimeRandomWalk;
        }

        agent.navMesh.destination = currentTarget;
    }

    // In order to get a random position around the agent i had to
    // search for navmesh functions that help me because i couldn't make
    // it work with a normal random position
    /// <see cref="https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html"/>
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
    private void ActivatePhysics(Agent agent)
    {
        agent.rigidbody.isKinematic = true;
        agent.navMesh.enabled = true;
        agent.navMesh.angularSpeed = 10000;
        agent.transform.rotation = Quaternion.identity;
    }

}
