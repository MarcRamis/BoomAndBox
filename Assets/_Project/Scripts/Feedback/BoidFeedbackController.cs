using UnityEngine;

public class BoidFeedbackController : FeedbackController
{
    public GameObject explosionPrefab;

    /////////// DIE
    public void PlayDeath()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }
}
