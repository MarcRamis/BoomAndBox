using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronManager : AIManager
{
    private bool idleTimerEnded = false;

    public override void OnEnter(Agent agent)
    {
        switch (agent.stateMachine.currentState)
        {
            case EAIState.RANDOM_WALK:
                break;
            case EAIState.CHASE_PLAYER:
                break;
            case EAIState.CHARGE:
                break;
            case EAIState.RECEIVE_DAMAGE:
                break;
            case EAIState.KEEP_DISTANCE:
                break;
            case EAIState.IDLE:

                Invoke(nameof(IdleToSeek), 1f);

                break;
            case EAIState.SEEK:
                
                agent.ChangeTargetDestination(seekTargets[0].position);

                break;
        }
    }

    public override void OnUpdate(Agent agent)
    {
        switch (agent.stateMachine.currentState)
        {
            case EAIState.RANDOM_WALK:

                if (IsPlayerNear(agent, agent.config.maxRandomWalkSightRadiusDistance) /*&& agent.playerScript.beingTargettedBy == null*/)
                {
                    agent.stateMachine.ChangeState(EAIState.CHASE_PLAYER);
                }

                else if (IsPlayerNear(agent, agent.config.maxRandomWalkSightRadiusDistance) && agent.playerScript.beingTargettedBy != null)
                {
                    // mantienes distancia
                }

                break;

            case EAIState.CHASE_PLAYER:

                //if (IsPlayerFarAway(agent, agent.config.maxDistanceToIdleState))
                //{
                //    agent.stateMachine.ChangeState(EAIState.RANDOM_WALK);
                //    return;
                //}

                if (IsPlayerNear(agent, agent.config.maxDistanceToChargeState))
                {
                    agent.stateMachine.ChangeState(EAIState.CHARGE);
                    return;
                }

                break;

            case EAIState.CHARGE:

                if (IsPlayerFarAway(agent, agent.config.maxDistanceToChaseState) && !agent.config.isNearlyCharge)
                {
                    agent.stateMachine.ChangeState(EAIState.CHASE_PLAYER);
                    return;
                }

                break;
            case EAIState.RECEIVE_DAMAGE:

                agent.stateMachine.ChangeState(EAIState.CHASE_PLAYER);

                break;
            case EAIState.KEEP_DISTANCE:
                break;
            case EAIState.IDLE:
                
                if (idleTimerEnded)
                {
                    agent.stateMachine.ChangeState(EAIState.SEEK);
                    idleTimerEnded = false;
                }

                break;
            case EAIState.SEEK:
                
                if (IsNear(agent, seekTargets[0], 1f))
                {
                    agent.stateMachine.ChangeState(EAIState.RANDOM_WALK);
                }

                break;
        }
    }
    public override void OnExit(Agent agent)
    {
    }

    private bool IsPlayerNear(Agent agent, float distance)
    {
        return Vector3.Distance(agent.player.transform.position, agent.transform.position) < distance;
    }

    private bool IsPlayerFarAway(Agent agent, float distance)
    {
        return Vector3.Distance(agent.player.transform.position, agent.transform.position) > distance;
    }

    private bool IsNear(Agent agent, Transform target, float distance)
    {
        return Vector3.Distance(target.position, agent.transform.position) < distance;
    }

    private void IdleToSeek()
    {
        idleTimerEnded = true;
        
    }
}
