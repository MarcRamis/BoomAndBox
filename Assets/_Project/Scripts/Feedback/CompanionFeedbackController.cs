using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CompanionFeedbackController : FeedbackController
{
    [SerializeField] private MMFeedbacks comebackFeedback; 
    [SerializeField] private MMFeedbacks exclamationFeedback;
    [SerializeField] private MMFeedbacks comebackingFeedback;
    [SerializeField] private Color throwDashColor;
    [SerializeField] private Color throwLargeColor;
    [SerializeField] private TrailRenderer trailRenderer;

    /////////// COMEBACK
    public void PlayComebackFeedback()
    {
        comebackFeedback.PlayFeedbacks();
    }
}