using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class HittableFeedbackController : FeedbackController
{
    [SerializeField] MMFeedbacks takeDamageFeedback;
    
    /////////// TAKE DAMAGE
    public void PlayTakeDamage()
    {
        takeDamageFeedback.PlayFeedbacks();
    }
}