using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    [SerializeField] public float maxTime = 1.0f;
    [SerializeField] public float maxDistance = 1.0f;
}