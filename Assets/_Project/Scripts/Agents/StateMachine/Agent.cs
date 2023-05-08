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
    [SerializeField] public GameObject player;
    [HideInInspector] public Player playerScript;
    [HideInInspector] public AIManager manager;
    [HideInInspector] public Vector3 direction;

    public bool isNavMeshAgent = true;

    protected void Awake()
    {
        if (isNavMeshAgent)
        {
            navMesh = GetComponent<NavMeshAgent>();

            manager = GetComponent<AIManager>();

            stateMachine = new AIStateMachine(this);
            stateMachine.RegisterState(new AIChasePlayerState());
            stateMachine.RegisterState(new AIRandomWalkState());
            stateMachine.RegisterState(new AIDronChargeState());
            stateMachine.RegisterState(new AIReceiveDamage());
            stateMachine.RegisterState(new AIKeepDistance());
            stateMachine.RegisterState(new AIIdleState());
            stateMachine.RegisterState(new AISeekState());
            stateMachine.ChangeState(initialState);
        }

        player = ReferenceSingleton.Instance.player;
        playerScript = ReferenceSingleton.Instance.playerScript;

        feedbackController = GetComponent<AgentFeedbackController>();
    }
    protected void Start()
    {
    }
    protected void Update()
    {
        direction = transform.forward;
    }

    protected void FixedUpdate()
    {
        stateMachine.Update();
    }
    
    public void RotateTo(Vector3 direction, float speedRotation)
    {
        Vector3 dir = direction - transform.position;
        dir = dir.normalized;
        
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.fixedDeltaTime * speedRotation);
    }
    
    public void ChangeTargetDestination(Vector3 pos)
    {
        navMesh.destination = pos;
    }
}