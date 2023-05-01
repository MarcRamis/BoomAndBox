using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDronManager : AIManager
{
    public override void OnEnter(Agent agent)
    {
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
                
                else if(IsPlayerNear(agent, agent.config.maxRandomWalkSightRadiusDistance) && agent.playerScript.beingTargettedBy != null)
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

}
