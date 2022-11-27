using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    [SerializeField] public float maxTimeChase = 1.0f;
    [SerializeField] public float maxDistance = 1.0f;
    [SerializeField] public float maxRandomWalkSightRadiusDistance = 10.0f;
    [SerializeField] public float maxRandomWalkRadius = 10.0f;
    [SerializeField] public float maxTimeRandomWalk = 2.0f;
    [SerializeField] public float maxPreparingSightRadiusDistance = 10.0f;
}