using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class PlayerFeedbackController : FeedbackController
{
    [SerializeField] private AudioClip receiveDamageSound;
    
    [Header("Visual Effects")]
    [Space]
    [SerializeField] private MMFeedbacks jumpFeedback;
    [SerializeField] private MMFeedbacks doubleJumpFeedback;
    [SerializeField] private MMFeedbacks landingFeedback;
    [SerializeField] private MMFeedbacks landingFeedbackShort;
    [SerializeField] private MMFeedbacks throwingFeedback;
    [SerializeField] private MMFeedbacks dashFeedback;
    [SerializeField] private MMFeedbacks receiveDamageFeedback;
    [SerializeField] private MMFeedbacks hitImpactFeedback;

    [Space]
    [SerializeField] private TrailRenderer trailLeftShoe;
    [SerializeField] private TrailRenderer trailRightShoe;
    [SerializeField] private TrailRenderer trailMidBody;
    [SerializeField] private TrailRenderer trailLeftHand;
    [SerializeField] private TrailRenderer trailRightHand;
    
    [Space]
    [SerializeField] private GameObject speedPs;
    [Space]
    [SerializeField] private Image cursor;
    [Space]
    [HideInInspector] private PlayerCharacterAnimations playerCharacterAnimations;

    private void Awake()
    {
        playerCharacterAnimations = GetComponent<PlayerCharacterAnimations>();
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
        trailLeftHand.emitting = true;
        trailRightHand.emitting = true;
        LargeShoesTrail();
    }
    public void StopDoubleJumpFeedback()
    {
        trailLeftHand.emitting = false;
        trailRightHand.emitting = false;
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
    /////////// RECEIVE DAMAGE
    public void PlayReceiveDamageFeedback()
    {
        receiveDamageFeedback.PlayFeedbacks();
        //PlaySoundEffect(receiveDamageSound);
        playerCharacterAnimations.PlayReceiveDamageAnimation();
    }

    /////////// START AIMING
    public void PlayAimingFeedback()
    {
        cursor.gameObject.SetActive(true);
    }
    public void StopAimingFeedback()
    {
        cursor.gameObject.SetActive(false);
    }

    /////////// Attack
    public void PlayAttack()
    {
        playerCharacterAnimations.PlayAttack();
        //trailLeftHand.emitting = true;
        //trailRightHand.emitting = true;
    }

    /////////// HIT IMPACT
    public void PlayHit()
    {
        hitImpactFeedback.PlayFeedbacks();
    }

    public void StopAttack()
    {
        trailLeftHand.emitting = false;
        trailRightHand.emitting = false;
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