using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CompanionFeedbackController : FeedbackController
{
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip beingThrownSound;
    [SerializeField] private AudioSource companionAudioSource;

    [Header("Visual Effects")]
    [SerializeField] private MMFeedbacks comebackFeedback; 
    [SerializeField] private MMFeedbacks exclamationFeedback;
    [SerializeField] private MMFeedbacks comebackingFeedback;
    [SerializeField] private MMFeedbacks beingThrownFeedback;

    [SerializeField] private Color throwDashColor;
    [SerializeField] private Color throwLargeColor;

    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private GameObject prefabHitExplosion;

    /////////// COMEBACK
    public void PlayComebackFeedback()
    {
        comebackFeedback.PlayFeedbacks();
    }
    /////////// HIT
    public void PlayHitFeedback(Vector3 contactPoint)
    {
        Instantiate(prefabHitExplosion, contactPoint, prefabHitExplosion.transform.rotation);
        PlaySoundEffect(hitSound);
    }
    /////////// BEING THROW
    public void PlayBeingThrownFeedback()
    {
        beingThrownFeedback.PlayFeedbacks();
        //PlaySoundEffect(beingThrownSound);
    }
}