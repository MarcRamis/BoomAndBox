using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public IAIState[] states; // Array to hold all the possible states
    public Agent agent; // Reference to the agent controlled by the state machine
    public EAIState currentState; // Current state of the state machine

    public AIStateMachine(Agent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(EAIState)).Length;
        states = new IAIState[numStates]; // Create an array to hold all the possible states
    }

    // Register a state with the state machine
    public void RegisterState(IAIState state)
    {
        int index = (int)state.GetId(); // Get the index of the state based on its ID
        states[index] = state; // Store the state in the array at the corresponding index
    }

    // Get a state based on its ID
    public IAIState GetState(EAIState stateID)
    {
        int index = (int)stateID; // Get the index of the state based on its ID
        return states[index]; // Return the state at the corresponding index
    }

    // Update the state machine
    public void Update()
    {
        GetState(currentState)?.Update(agent); // Get the current state and update it with the agent
    }

    // Change the state of the state machine
    public void ChangeState(EAIState newState)
    {
        GetState(currentState)?.Exit(agent); // Exit the current state (if any)
        currentState = newState; // Set the new state
        GetState(currentState)?.Enter(agent); // Enter the new state
    }

}
