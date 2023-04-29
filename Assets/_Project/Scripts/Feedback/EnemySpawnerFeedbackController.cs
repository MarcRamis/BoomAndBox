using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EnemySpawnerFeedbackController : MonoBehaviour
{
    [SerializeField] private MMFeedbacks moveFeedback;
    [SerializeField] private MMFeedbacks openFeedback;
    [SerializeField] private MMFeedbacks closeFeedback;
    [SerializeField] private MMFeedbacks hideFeedback;
    [SerializeField] private MMFeedbacks enemyMoveFeedback;

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
        closeFeedback.PlayFeedbacks();
    }

    public void PlayHideFeedback()
    {
        hideFeedback.PlayFeedbacks();
    }

    public void PlayEnemyMoveFeedback()
    {
        enemyMoveFeedback.PlayFeedbacks();
    }

}
