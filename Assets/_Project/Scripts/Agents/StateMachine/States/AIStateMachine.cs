using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public IAIState[] states;
    public Agent agent;
    public EAIState currentState;

    public AIStateMachine(Agent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(EAIState)).Length;
        states = new IAIState[numStates];
    }

    public void RegisterState(IAIState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public IAIState GetState(EAIState stateID)
    {
        int index = (int)stateID;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(EAIState newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}