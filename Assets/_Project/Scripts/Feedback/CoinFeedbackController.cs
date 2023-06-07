using UnityEngine;
using MoreMountains.Feedbacks;

public class CoinFeedbackController : FeedbackController
{
    [SerializeField] private GameObject pickFeedback;
    [SerializeField] private MMFeedbacks soundFeedback;
    
    public void PlayTakenItem()
    {
        Instantiate(pickFeedback, transform.position, transform.rotation);
        soundFeedback.PlayFeedbacks();
    }
}