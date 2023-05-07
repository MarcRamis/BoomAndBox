using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using UnityEngine.Rendering.Universal;

public class PlayerFeedbackController : FeedbackController
{
    [SerializeField] private AudioClip receiveDamageSound;
    [SerializeField] private AudioClip attackSound;
    
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
    [SerializeField] private MMFeedbacks attackFeedback;
    [SerializeField] private MMFeedbacks rhythmFeedback;

    [Space]
    [SerializeField] private TrailRenderer trailLeftShoe;
    [SerializeField] private TrailRenderer trailRightShoe;
    [SerializeField] private TrailRenderer trailMidBody;
    [SerializeField] private TrailRenderer trailLeftHand;
    [SerializeField] private TrailRenderer trailRightHand;
    
    [Space]
    [SerializeField] private GameObject speedPs;
    [SerializeField] private GameObject rythmFoot;
    [SerializeField] private DecalProjector rythmShadow;
    [SerializeField] private GameObject rythmCombo;
    [SerializeField] private GameObject splashPrefab;

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
        PlaySoundEffect(receiveDamageSound, 0.5f);
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
    public void PlayAttack(int counter)
    {
        PlaySoundEffect(attackSound, 1.5f);
        playerCharacterAnimations.PlayAttack(counter);
        attackFeedback.PlayFeedbacks();
        //trailLeftHand.emitting = true;
        //trailRightHand.emitting = true;
    }

    /////////// RYTHM
    public void PlayRythmMoment()
    {

        splashPrefab.SetActive(true);
        rythmFoot.SetActive(true);
        rythmShadow.size = new Vector3(4,4, rythmShadow.size.z);
    }

    public void StopRythmMoment()
    {
        splashPrefab.SetActive(false);
        rythmFoot.SetActive(false);
        rythmShadow.size = new Vector3(2, 2, rythmShadow.size.z);
    }

    /////////// RYTHM OPPORTUNITY ADQUIRED
    public void PlayRythmed()
    {
        rhythmFeedback.PlayFeedbacks();
        rythmCombo.SetActive(true);
    }

    public void StopRythmed()
    {
        rythmCombo.SetActive(false);
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