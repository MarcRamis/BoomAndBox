using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CompanionFeedbackController : FeedbackController
{
    [SerializeField] private MMFeedbacks comebackFeedback; 
    [SerializeField] private MMFeedbacks exclamationFeedback;

    /////////// COMEBACK
    public void PlayComebackFeedback()
    {
        comebackFeedback.PlayFeedbacks();
    }
}