using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerFeedbackController : FeedbackController
{
    // was in movement script
    [SerializeField] private MMFeedbacks jumpFeedback;
    [SerializeField] private MMFeedbacks doubleJumpFeedback;
    [SerializeField] private MMFeedbacks landingFeedback;
    [SerializeField] private MMFeedbacks landingFeedbackShort;
    [SerializeField] private TrailRenderer trailLeftShoe;
    [SerializeField] private TrailRenderer trailRightShoe;
    // was in dash script
    [SerializeField] private GameObject speedPs;
    [SerializeField] private MMFeedbacks dashFeedback;
    [SerializeField] private TrailRenderer trailMidBody;
    // was in throwing script
    [SerializeField] private MMFeedbacks throwingFeedback;

    
    [SerializeField] private MMFeedbacks comebackFeedback; //companion feedback
    [SerializeField] private MMFeedbacks exclamationFeedback; // companion feedback

    private void Awake()
    {
        speedPs.SetActive(false);
    }

    /////////// JUMP
    public void PlayJumpFeedback()
    {
        jumpFeedback.PlayFeedbacks();
        LargeShoesTrail();
    }
    public void StopJumpFeedback()
    {
        ShortShoesTrail();
    }

    /////////// DOUBLE JUMP
    public void PlayDoubleJumpFeedback()
    {
        doubleJumpFeedback.PlayFeedbacks();
        LargeShoesTrail();
    }
    public void StopDoubleJumpFeedback()
    {
        ShortShoesTrail();
    }
    /////////// DASH
    public void PlayDashFeedback()
    {
        dashFeedback.PlayFeedbacks();
        LargeShoesTrail();
        trailMidBody.emitting = true;
        speedPs.SetActive(true);
    }
    public void StopDashFeedback()
    {
        ShortShoesTrail();
        trailMidBody.emitting = false;
        speedPs.SetActive(false);
    }
    /////////// LAND
    public void PlayLandingShortFeedback()
    {
        landingFeedbackShort.PlayFeedbacks();
    }
    public void PlayLandingLargeFeedback()
    {
        landingFeedback.PlayFeedbacks();
    }
    /////////// THROW
    public void PlayThrowFeedback()
    {
        throwingFeedback.PlayFeedbacks();
    }

    private void LargeShoesTrail()
    {
        trailLeftShoe.emitting = true;
        trailRightShoe.emitting = true;

        trailLeftShoe.widthMultiplier = 0.30f;
        trailRightShoe.widthMultiplier = 0.30f;

        trailLeftShoe.time = 0.3f;
        trailRightShoe.time = 0.3f;
    }

    private void ShortShoesTrail()
    {
        trailLeftShoe.emitting = true;
        trailRightShoe.emitting = true;

        trailLeftShoe.widthMultiplier = 0.10f;
        trailRightShoe.widthMultiplier = 0.10f;

        trailLeftShoe.time = 0.04f;
        trailRightShoe.time = 0.04f;
    }
}