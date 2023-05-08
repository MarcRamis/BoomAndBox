using UnityEngine;

public class BoidFeedbackController : AgentFeedbackController
{
    public GameObject explosionPrefab;

    private void Start()
    {
        Invoke(nameof(PlayDeath),5f);
    }

    /////////// DIE
    public override void PlayDeath()
    {
        Instantiate(explosionPrefab, transform);
    }
}
