using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    [SerializeField] protected AiBehaviour initialState;
    protected AIStateMachine stateMachine;
    
    protected void Start()
    {
        stateMachine = new AIStateMachine(this);
        stateMachine.ChangeState(initialState);
    }
    
    protected void FixedUpdate()
    {
        stateMachine.Update();
        Debug.Log("a");
    }
}