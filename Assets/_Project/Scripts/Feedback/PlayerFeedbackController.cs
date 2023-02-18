using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerFeedbackController : FeedbackController
{
    [SerializeField] private MMFeedbacks jumpFeedback;
    [SerializeField] private MMFeedbacks doubleJumpFeedback;
    [SerializeField] private MMFeedbacks landingFeedback;
    [SerializeField] private MMFeedbacks landingFeedbackShort;
    [SerializeField] private TrailRenderer trailLeftShoe;
    [SerializeField] private TrailRenderer trailRightShoe;

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
        LargeShoesTrail();
    }
    public void StopDashFeedback()
    {
        ShortShoesTrail();
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