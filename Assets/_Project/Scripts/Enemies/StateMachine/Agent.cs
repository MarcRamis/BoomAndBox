using UnityEngine;
using UnityEngine.AI;

public abstract class Agent : MonoBehaviour
{
    [Header("Agent Settings")]
    [SerializeField] public EAIState initialState;
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMesh;
    [SerializeField] public AIAgentConfig config;
    [SerializeField] public new Rigidbody rigidbody;
    [HideInInspector] public AgentFeedbackController feedbackController;
    [HideInInspector] public GameObject player;
    [HideInInspector] public AIManager manager;
    
    protected void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIRandomWalkState());
        stateMachine.RegisterState(new AIDronChargeState());
        stateMachine.ChangeState(initialState);

        player = GameObject.FindGameObjectWithTag("Player");

        feedbackController = GetComponent<AgentFeedbackController>();
        
        manager = GetComponent<AIManager>();
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