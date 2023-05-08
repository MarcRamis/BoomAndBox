using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System;

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
    [SerializeField] private GameObject fakeShadow;

    /////////// COMEBACK
    public void PlayComeback()
    {
        comebackFeedback.PlayFeedbacks();
    }
    /////////// COMEBACKING
    public void PlayComebacking()
    {
        comebackingFeedback.PlayFeedbacks();
    }

    /////////// HIT
    public void PlayHit(Vector3 contactPoint)
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

    /////////// THROW AGAIN
    public void PlayThrowAgain()
    {
        exclamationFeedback.PlayFeedbacks();
    }

    /////////// Trail
    public void PlayLargeTrail()
    {
        trailRenderer.endColor = throwLargeColor;
        trailRenderer.startColor = throwLargeColor;
    }

    public void PlayShortTrail()
    {
        trailRenderer.endColor = throwDashColor;
        trailRenderer.startColor = throwDashColor;
    }
    
    /////////// Shadow
    public void ShowShadow()
    {
        fakeShadow.SetActive(true);
    }
    public void HideShadow()
    {
        fakeShadow.SetActive(false);
    }
}