using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EnemySpawnerFeedbackController : MonoBehaviour
{
    [SerializeField] private MMFeedbacks moveFeedback;
    [SerializeField] private MMFeedbacks openFeedback;
    [SerializeField] private MMFeedbacks hideFeedback;

    public void PlayMoveFeedback()
    {
        moveFeedback.PlayFeedbacks();
    }

    public void PlayOpenFeedback()
    {
        openFeedback.PlayFeedbacks();
    }

    public void PlayCloseFeedback()
    {
        openFeedback.PlayFeedbacksInReverse();
    }

    public void PlayHideFeedback()
    {
        hideFeedback.PlayFeedbacks();
    }
}
