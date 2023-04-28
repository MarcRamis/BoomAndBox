using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    // Chase Player
    [SerializeField] public float maxTimeChase = 1.0f;
    [SerializeField] public float maxDistance = 1.0f;
    [SerializeField] public float maxDistanceToIdleState = 15.0f;
    [SerializeField] public float maxDistanceToChargeState = 5.0f;
    [SerializeField] public float chaseSpeed = 5.0f;
    [SerializeField] public float chaseAngularSpeed = 800;
    
    // MOVE
    [SerializeField] public float maxRandomWalkSightRadiusDistance = 10.0f;
    [SerializeField] public float maxRandomWalkRadius = 10.0f;
    [SerializeField] public float maxTimeRandomWalk = 2.0f;
    [SerializeField] public float idleSpeed = 5.0f;
    [SerializeField] public float idleAngularSpeed = 400;
    
    // Charge
    [SerializeField] public float chargeForce = 100.0f;
    [SerializeField] public float timePreparingToCharge = 2.0f;
    [SerializeField] public float maxDistanceToChaseState = 5.0f;
    [SerializeField] public float rotationSpeed = 10f;
    [HideInInspector] public bool isNearlyCharge;
}