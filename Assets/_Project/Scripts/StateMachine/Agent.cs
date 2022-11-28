using UnityEngine;
using UnityEngine.AI;

public abstract class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    [SerializeField] public EAIState initialState;
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMesh;
    [SerializeField] public AIAgentConfig config;
    [SerializeField] public Rigidbody rigidbody;

    protected void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIRandomWalkState());
        stateMachine.RegisterState(new AIDronChargeState());
        stateMachine.ChangeState(initialState);
    }
    protected void Start()
    {
    }
    protected void Update()
    {

    }

    protected void FixedUpdate()
    {
        stateMachine.Update();
    }
}