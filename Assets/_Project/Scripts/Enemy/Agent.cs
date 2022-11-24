using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateID initialState;

    protected void Start()
    {
        stateMachine = new AIStateMachine(this);
        stateMachine.ChangeState(initialState);
    }

    protected void Update()
    {
        stateMachine.Update();
    }
}