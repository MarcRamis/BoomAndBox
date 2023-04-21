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
    
    protected void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIRandomWalkState());
        stateMachine.RegisterState(new AIDronChargeState());
        stateMachine.RegisterState(new AIReceiveDamage());
        stateMachine.RegisterState(new AIKeepDistance());
        stateMachine.ChangeState(initialState);
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

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
    
    public void RotateTo(Vector3 direction, float speedRotation)
    {
        Vector3 dir = direction - transform.position;
        dir = dir.normalized;
        
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.fixedDeltaTime * speedRotation);
    }
}