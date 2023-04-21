using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class DronFeedbackController : AgentFeedbackController
{
    [Header("Visual Effects")]
    [SerializeField] private MMFeedbacks damageFeedback;
    [SerializeField] private MMFeedbacks dieFeedback;
    [SerializeField] private MMFeedbacks preparingForChargeFeedback;
    [SerializeField] private MMFeedbacks chargeFeedback;
    [SerializeField] private MMFeedbacks walkFeedback;
    [SerializeField] private MMFeedbacks runFeedback;
    
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject dronChargingPrefab;

    [SerializeField] private TrailRenderer trail1;
    [SerializeField] private TrailRenderer trail2;
    [SerializeField] private TrailRenderer trail3;

    /////////// TAKE DAMAGE
    public override void PlayTakeDamage()
    {
        damageFeedback.PlayFeedbacks();
    }

    /////////// DIE
    public override void PlayDeath()
    {
        dieFeedback.PlayFeedbacks();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    /////////// PREPARING CHARGE
    public override void PlayPreparingCharge()
    {
        dronChargingPrefab.SetActive(true);
        //preparingForChargeFeedback.PlayFeedbacks();
    }
    
    public override void StopPreparingCharge()
    {
        dronChargingPrefab.SetActive(false);
        //preparingForChargeFeedback.StopFeedbacks();
    }

    /////////// CHARGE
    public override void PlayCharge()
    {
        trail1.emitting = true;
        trail2.emitting = true;
        trail3.emitting = true;
        chargeFeedback.PlayFeedbacks();
    }

    public override void StopCharge()
    {
        trail1.emitting = false;
        trail2.emitting = false;
        trail3.emitting = false;
        chargeFeedback.StopFeedbacks();
    }

    /////////// IMPULSE
    public override void PlayImpulse()
    {
    }
    
    /////////// IMPULSE
    public override void PlayWalk()
    {
        walkFeedback.PlayFeedbacks();
    }
    /////////// RUN
    public override void PlayRun()
    {
        runFeedback.PlayFeedbacks();
    }
}